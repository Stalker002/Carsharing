using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface IFinesService
{
    Task<List<Fine>> GetFines(CancellationToken cancellationToken);

    Task<List<Fine>> GetPagedFines(int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCountFines(CancellationToken cancellationToken);

    Task<List<Fine>> GetFineById(int id, CancellationToken cancellationToken);

    Task<List<Fine>> GetFinesByTripId(int tripId, CancellationToken cancellationToken);

    Task<List<Fine>> GetFinesByTripId(int userId, int tripId, CancellationToken cancellationToken);

    Task<List<Fine>> GetFinesByStatusId(int statusId, CancellationToken cancellationToken);

    Task<int> CreateFine(Fine fine, CancellationToken cancellationToken);

    Task<int> UpdateFine(int id, int? tripId, int? statusId, string? type, decimal? amount,
        DateTime? date, CancellationToken cancellationToken);

    Task<int> DeleteFine(int id, CancellationToken cancellationToken);
}