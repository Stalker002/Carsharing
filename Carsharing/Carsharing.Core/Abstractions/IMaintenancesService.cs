using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IMaintenancesService
{
    Task<List<Maintenance>> GetMaintenances();
    Task<int> CreateMaintenance(Maintenance maintenance);

    Task<int> UpdateMaintenance(int id, int? carId, string? workType, string? description, decimal? cost,
        DateOnly? date);

    Task<int> DeleteMaintenance(int id);
}