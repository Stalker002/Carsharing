using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IFinesService
{
    Task<List<Fine>> GetFines();
    Task<int> CreateFine(Fine fine);

    Task<int> UpdateFine(int id, int? tripId, int? statusId, string? type, decimal? amount,
        DateOnly? date);

    Task<int> DeleteFine(int id);
}