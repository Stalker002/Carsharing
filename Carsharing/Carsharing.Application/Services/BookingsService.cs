using Carsharing.Application.Abstractions;
using Carsharing.Application.DTOs;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class BookingsService : IBookingsService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IClientRepository _clientRepository;
    private readonly ICarRepository _carRepository;
    private readonly IBookingStatusRepository _statusRepository;
    private readonly ISpecificationCarRepository _specificationCarRepository;

    public BookingsService(IBookingRepository bookingRepository, IClientRepository clientRepository,
        ICarRepository carRepository, IBookingStatusRepository statusRepository,
        ISpecificationCarRepository specificationCarRepository)
    {
        _specificationCarRepository = specificationCarRepository;
        _statusRepository = statusRepository;
        _carRepository = carRepository;
        _clientRepository = clientRepository;
        _bookingRepository = bookingRepository;
    }

    //GetByUserId

    public async Task<List<Booking>> GetBookings()
    {
        return await _bookingRepository.Get();
    }

    public async Task<List<Booking>> GetPagedBookings(int page, int limit)
    {
        return await _bookingRepository.GetPaged(page, limit);
    }

    public async Task<int> GetCountBookings()
    {
        return await _bookingRepository.GetCount();
    }

    public async Task<List<Booking>> GetBookingsById(int id)
    {
        return await _bookingRepository.GetById(id);
    }

    public async Task<List<Booking>> GetBookingsByClient(int userId)
    {
        var client = await _clientRepository.GetClientByUserId(userId);
        var clientId = client.Select(c => c.Id).FirstOrDefault();

        return await _bookingRepository.GetByClientId(clientId);
    }

    public async Task<List<Booking>> GetPagedBookingsByClient(int userId, int page, int limit)
    {
        var client = await _clientRepository.GetClientByUserId(userId);
        var clientId = client.Select(c => c.Id).FirstOrDefault();

        return await _bookingRepository.GetPagedByClientId(clientId, page, limit);
    }

    public async Task<int> GetCountBookingsByClient(int userId)
    {
        var client = await _clientRepository.GetClientByUserId(userId);
        var clientId = client.Select(c => c.Id).FirstOrDefault();

        return await _bookingRepository.GetCountByClient(clientId);
    }

    public async Task<List<Booking>> GetBookingsByCarId(int carId)
    {
        return await _bookingRepository.GetByCarId(carId);
    }

    public async Task<List<BookingWithFullInfoDto>> GetBookingWithInfo(int id)
    {
        var bookings = await _bookingRepository.GetById(id);
        if (bookings.Count == 0) return [];
        var clientId = bookings.Select(b => b.ClientId).FirstOrDefault();
        var carId = bookings.Select(b => b.CarId).FirstOrDefault();

        var clients = await _clientRepository.GetById(clientId);

        var cars = await _carRepository.GetById(carId);
        var specificationId = cars.Select(c => c.SpecificationId).FirstOrDefault();

        var specification = await _specificationCarRepository.GetById(specificationId);

        var statuses = await _statusRepository.Get();

        var response =
            (from b in bookings
             join c in cars on b.CarId equals c.Id
             join s in statuses on b.StatusId equals s.Id
             join cl in clients on b.ClientId equals cl.Id
             join sp in specification on c.SpecificationId equals sp.Id
             select new BookingWithFullInfoDto(
                 b.Id,
                 s.Name,
                 $"{clients.First().Name} {clients.First().Surname}",
                 $"{sp.Brand} {sp.Model} ({sp.StateNumber})",
                 b.StartTime,
                 b.EndTime
             )).ToList();

        return response;
    }

    public async Task<int> CreateBooking(Booking booking)
    {
        return await _bookingRepository.Create(booking);
    }

    public async Task<int> UpdateBooking(int id, int? statusId, int? carId, int? clientId,
        DateTime? startTime, DateTime? endTime)
    {
        return await _bookingRepository.Update(id, statusId, carId, clientId, startTime, endTime);
    }

    public async Task<int> DeleteBooking(int id)
    {
        return await _bookingRepository.Delete(id);
    }
}