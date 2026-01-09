using Carsharing.Application.DTOs;
using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface IBookingsService
{
    Task<List<Booking>> GetBookings();
    Task<List<Booking>> GetPagedBookings(int page, int limit);
    Task<int> GetCountBookings();
    Task<List<Booking>> GetBookingsById(int id);
    Task<List<Booking>> GetBookingsByClient(int userId);
    Task<List<Booking>> GetPagedBookingsByClient(int userId, int page, int limit);
    Task<int> GetCountBookingsByClient(int userId);
    Task<List<Booking>> GetBookingsByCarId(int carId);
    Task<List<BookingWithFullInfoDto>> GetBookingWithInfo(int id);
    Task<int> CreateBooking(Booking booking);
    Task<int> UpdateBooking(int id, int? statusId, int? carId, int? clientId,
        DateTime? startTime, DateTime? endTime);
    Task<int> DeleteBooking(int id);
}