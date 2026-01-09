using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IBookingStatusRepository
{
    Task<List<BookingStatus>> Get();
    Task<bool> Exists(int id);
}