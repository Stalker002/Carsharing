using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Exceptions;
using Carsharing.Core.Models;
using Shared.Contracts.Bookings;
using Shared.Enums;

namespace Carsharing.Application.Services;

public class BookingsService : IBookingsService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ICarRepository _carRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BookingsService(IBookingRepository bookingRepository, IClientRepository clientRepository,
        ICarRepository carRepository, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _carRepository = carRepository;
        _clientRepository = clientRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task<List<Booking>> GetBookings(CancellationToken cancellationToken)
    {
        return await _bookingRepository.Get(cancellationToken);
    }

    public async Task<List<Booking>> GetPagedBookings(int page, int limit, CancellationToken cancellationToken)
    {
        return await _bookingRepository.GetPaged(page, limit, cancellationToken);
    }

    public async Task<int> GetCountBookings(CancellationToken cancellationToken)
    {
        return await _bookingRepository.GetCount(cancellationToken);
    }

    public async Task<List<Booking>> GetBookingsById(int id, CancellationToken cancellationToken)
    {
        return await _bookingRepository.GetById(id, cancellationToken);
    }

    public async Task<List<Booking>> GetBookingsByClient(int userId, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetClientByUserId(userId, cancellationToken);
        var clientId = client.Select(c => c.Id).FirstOrDefault();

        return await _bookingRepository.GetByClientId(clientId, cancellationToken);
    }

    public async Task<List<Booking>> GetPagedBookingsByClient(int userId, int page, int limit,
        CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetClientByUserId(userId, cancellationToken);
        var clientId = client.Select(c => c.Id).FirstOrDefault();

        return await _bookingRepository.GetPagedByClientId(clientId, page, limit, cancellationToken);
    }

    public async Task<int> GetCountBookingsByClient(int userId, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetClientByUserId(userId, cancellationToken);
        var clientId = client.Select(c => c.Id).FirstOrDefault();

        return await _bookingRepository.GetCountByClient(clientId, cancellationToken);
    }

    public async Task<List<Booking>> GetBookingsByCarId(int carId, CancellationToken cancellationToken)
    {
        return await _bookingRepository.GetByCarId(carId, cancellationToken);
    }

    public async Task<List<Booking>> GetBookingsByCarId(int userId, int carId, CancellationToken cancellationToken)
    {
        var clientId = await GetRequiredClientId(userId, cancellationToken);
        var bookings = await _bookingRepository.GetByCarId(carId, cancellationToken);

        return bookings.Where(b => b.ClientId == clientId).ToList();
    }

    public async Task<List<BookingWithFullInfoDto>> GetBookingWithInfo(int id, CancellationToken cancellationToken)
    {
        return await _bookingRepository.GetBookingWithInfo(id, cancellationToken);
    }

    public async Task<List<BookingWithFullInfoDto>> GetBookingWithInfo(int userId, int id,
        CancellationToken cancellationToken)
    {
        var clientId = await GetRequiredClientId(userId, cancellationToken);
        var booking = (await _bookingRepository.GetById(id, cancellationToken)).FirstOrDefault()
                      ?? throw new NotFoundException("Booking not found");

        if (booking.ClientId != clientId)
            throw new UnauthorizedAccessException("Booking does not belong to current user");

        return await _bookingRepository.GetBookingWithInfo(id, cancellationToken);
    }

    public async Task<int> CreateBooking(int userId, int statusId, int carId, DateTime startTime, DateTime endTime,
        CancellationToken cancellationToken)
    {
        var clientId = await GetRequiredClientId(userId, cancellationToken);

        var (booking, error) = Booking.Create(0, statusId, carId, clientId, startTime, endTime);

        if (!string.IsNullOrWhiteSpace(error))
            throw new ArgumentException(error);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var wasReserved = await _carRepository.TryUpdateStatus(
                booking.CarId,
                (int)CarStatusEnum.Available,
                (int)CarStatusEnum.Reserved,
                cancellationToken);

            if (!wasReserved)
            {
                var cars = await _carRepository.GetById(booking.CarId, cancellationToken);
                var car = cars.FirstOrDefault();

                if (car == null)
                    throw new NotFoundException("Автомобиль не найден");

                throw new ConflictException("Автомобиль недоступен на выбранное время");
            }

            var bookingId = await _bookingRepository.Create(booking, cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return bookingId;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<int> UpdateBooking(int id, int? statusId, int? carId, int? clientId,
        DateTime? startTime, DateTime? endTime, CancellationToken cancellationToken)
    {
        return await _bookingRepository.Update(id, statusId, carId, clientId, startTime, endTime, cancellationToken);
    }

    public async Task<int> DeleteBooking(int id, CancellationToken cancellationToken)
    {
        return await _bookingRepository.Delete(id, cancellationToken);
    }

    private async Task<int> GetRequiredClientId(int userId, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetClientByUserId(userId, cancellationToken);
        var clientId = client.Select(c => c.Id).FirstOrDefault();

        if (clientId == 0)
            throw new NotFoundException("Клиент не найден");

        return clientId;
    }
}