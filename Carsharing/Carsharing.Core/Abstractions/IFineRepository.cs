using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IFineRepository
{
    Task<List<Fine>> Get(CancellationToken cancellationToken);

    Task<List<Fine>> GetPaged(int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCount(CancellationToken cancellationToken);

    Task<List<Fine>> GetById(int id, CancellationToken cancellationToken);

    Task<List<Fine>> GetByTripId(int tripId, CancellationToken cancellationToken);

    Task<List<Fine>> GetByStatusId(int statusId, CancellationToken cancellationToken);

    Task<int> Create(Fine fine, CancellationToken cancellationToken);

    Task<int> Update(int id, int? tripId, int? statusId, string? type, decimal? amount,
        DateTime? date, CancellationToken cancellationToken);

    Task<int> Delete(int id, CancellationToken cancellationToken);
}