using Carsharing.Application.Abstractions;
using Carsharing.Application.DTOs;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Enum;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class BookingsService : IBookingsService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IClientRepository _clientRepository;
    private readonly ICarRepository _carRepository;

    public BookingsService(IBookingRepository bookingRepository, IClientRepository clientRepository,
        ICarRepository carRepository)
    {
        _carRepository = carRepository;
        _clientRepository = clientRepository;
        _bookingRepository = bookingRepository;
    }

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
        return await _bookingRepository.GetBookingWithInfo(id);
    }

    public async Task<int> CreateBooking(Booking booking)
    {
        await _carRepository?.UpdateStatus(
            booking.CarId, (int)CarStatusEnum.Reserved
        )!;

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