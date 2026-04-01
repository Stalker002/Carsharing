using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IBillStatusRepository
{
    Task<List<BillStatus>> Get(CancellationToken cancellationToken);

    Task<bool> Exists(int id, CancellationToken cancellationToken);
}