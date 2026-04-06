using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface ITripStatusRepository
{
    Task<List<TripStatus>> Get(CancellationToken cancellationToken);

    Task<bool> Exists(int id, CancellationToken cancellationToken);
}