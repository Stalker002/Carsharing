using Carsharing.Application.DTOs;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public interface IBookingsService
{
    Task<List<Booking>> GetBookings();
    Task<List<Booking>> GetPagedBookings(int page, int limit);
    Task<int> GetCountBookings();
    Task<List<Booking>> GetBookingsById(int id);
    Task<List<Booking>> GetBookingsByClientId(int clientId);
    Task<List<Booking>> GetPagedBookingsByClientId(int clientId, int page, int limit);
    Task<int> GetCountBookingsByClientId(int clientId);
    Task<List<Booking>> GetBookingsByCarId(int carId);
    Task<List<BookingWithFullInfoDto>> GetBookingWithInfo(int id);
    Task<int> CreateBooking(Booking booking);
    Task<int> UpdateBooking(int id, int? statusId, int? carId, int? clientId,
        DateTime? startTime, DateTime? endTime);
    Task<int> DeleteBooking(int id);
}