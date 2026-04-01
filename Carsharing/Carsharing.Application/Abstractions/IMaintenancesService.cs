using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface IMaintenancesService
{
    Task<List<Maintenance>> GetMaintenances(CancellationToken cancellationToken);

    Task<List<Maintenance>> GetMaintenanceById(int id, CancellationToken cancellationToken);

    Task<List<Maintenance>> GetMaintenanceByCarId(int carId, CancellationToken cancellationToken);

    Task<List<Maintenance>> GetByDateRange(DateOnly from, DateOnly to, CancellationToken cancellationToken);

    Task<int> CreateMaintenance(Maintenance maintenance, CancellationToken cancellationToken);

    Task<int> UpdateMaintenance(int id, int? carId, string? workType, string? description, decimal? cost,
        DateOnly? date, CancellationToken cancellationToken);

    Task<int> DeleteMaintenance(int id, CancellationToken cancellationToken);
}