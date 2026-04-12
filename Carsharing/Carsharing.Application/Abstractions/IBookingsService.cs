using Carsharing.Core.Models;
using Shared.Contracts.Bookings;

namespace Carsharing.Application.Abstractions;

public interface IBookingsService
{
    Task<List<Booking>> GetBookings(CancellationToken cancellationToken);

    Task<List<Booking>> GetPagedBookings(int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCountBookings(CancellationToken cancellationToken);

    Task<List<Booking>> GetBookingsById(int id, CancellationToken cancellationToken);

    Task<List<Booking>> GetBookingsByClient(int userId, CancellationToken cancellationToken);

    Task<List<Booking>> GetPagedBookingsByClient(int userId, int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCountBookingsByClient(int userId, CancellationToken cancellationToken);

    Task<List<Booking>> GetBookingsByCarId(int carId, CancellationToken cancellationToken);

    Task<List<Booking>> GetBookingsByCarId(int userId, int carId, CancellationToken cancellationToken);

    Task<List<BookingWithFullInfoDto>> GetBookingWithInfo(int id, CancellationToken cancellationToken);

    Task<List<BookingWithFullInfoDto>> GetBookingWithInfo(int userId, int id, CancellationToken cancellationToken);

    Task<int> CreateBooking(int userId, int statusId, int carId, DateTime startTime, DateTime endTime, CancellationToken cancellationToken);

    Task<int> UpdateBooking(int id, int? statusId, int? carId, int? clientId,
        DateTime? startTime, DateTime? endTime, CancellationToken cancellationToken);

    Task<int> DeleteBooking(int id, CancellationToken cancellationToken);
}
