using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class BookingsService : IBookingsService
{
    private readonly IBookingRepository _bookingRepository;

    public BookingsService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    //GetByUserId

    public async Task<List<Booking>> GetBookings()
    {
        return await _bookingRepository.Get();
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