using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IBookingStatusRepository
{
    Task<List<BookingStatus>> Get(CancellationToken cancellationToken);

    Task<bool> Exists(int id, CancellationToken cancellationToken);
}