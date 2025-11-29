using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IFinesService
{
    Task<List<Fine>> GetFines();
    Task<List<Fine>> GetPagedFines(int page, int limit);
    Task<int> GetCountFines();
    Task<List<Fine>> GetFineById(int id);
    Task<List<Fine>> GetFinesByTripId(int tripId);
    Task<List<Fine>> GetFinesByStatusId(int statusId);
    Task<int> CreateFine(Fine fine);

    Task<int> UpdateFine(int id, int? tripId, int? statusId, string? type, decimal? amount,
        DateOnly? date);

    Task<int> DeleteFine(int id);
}