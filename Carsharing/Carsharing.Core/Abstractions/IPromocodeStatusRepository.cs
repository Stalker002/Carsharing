using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IPromocodeStatusRepository
{
    Task<List<PromocodeStatus>> Get(CancellationToken cancellationToken);

    Task<bool> Exists(int id, CancellationToken cancellationToken);
}