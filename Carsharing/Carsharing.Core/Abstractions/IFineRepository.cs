using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IFineRepository
{
    Task<List<Fine>> Get();
    Task<List<Fine>> GetPaged(int page, int limit);
    Task<int> GetCount();
    Task<List<Fine>> GetById(int id);
    Task<List<Fine>> GetByTripId(int tripId);
    Task<List<Fine>> GetByStatusId(int statusId);
    Task<int> Create(Fine fine);

    Task<int> Update(int id, int? tripId, int? statusId, string? type, decimal? amount,
        DateTime? date);

    Task<int> Delete(int id);
}