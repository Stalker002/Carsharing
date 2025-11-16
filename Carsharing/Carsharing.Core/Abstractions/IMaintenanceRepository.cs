using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IMaintenanceRepository
{
    Task<List<Maintenance>> Get();
    Task<int> Create(Maintenance maintenance);

    Task<int> Update(int id, int? carId, string? workType, string? description, decimal? cost,
        DateOnly? date);

    Task<int> Delete(int id);
}