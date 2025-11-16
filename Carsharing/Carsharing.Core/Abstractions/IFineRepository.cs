using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IFineRepository
{
    Task<List<Fine>> Get();
    Task<int> Create(Fine fine);

    Task<int> Update(int id, int? tripId, int? statusId, string? type, decimal? amount,
        DateOnly? date);

    Task<int> Delete(int id);
}