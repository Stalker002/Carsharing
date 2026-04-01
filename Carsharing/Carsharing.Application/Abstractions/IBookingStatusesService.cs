using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface IBookingStatusesService
{
    Task<List<BookingStatus>> GetBookingStatuses();
}