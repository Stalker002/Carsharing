using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IBookingsService
{
    Task<List<Booking>> GetBookings();
    Task<int> CreateBooking(Booking booking);

    Task<int> UpdateBooking(int id, int? statusId, int? carId, int? clientId,
        DateTime? startTime, DateTime? endTime);

    Task<int> DeleteBooking(int id);
}