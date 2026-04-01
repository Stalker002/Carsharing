using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IFineStatusRepository
{
    Task<List<FineStatus>> Get(CancellationToken cancellationToken);

    Task<bool> Exists(int id, CancellationToken cancellationToken);
}