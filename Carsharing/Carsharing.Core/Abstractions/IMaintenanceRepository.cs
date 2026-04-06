using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IMaintenanceRepository
{
    Task<List<Maintenance>> Get(CancellationToken cancellationToken);

    Task<List<Maintenance>> GetById(int id, CancellationToken cancellationToken);

    Task<List<Maintenance>> GetByCarId(int carId, CancellationToken cancellationToken);

    Task<List<Maintenance>> GetByDateRange(DateOnly from, DateOnly to, CancellationToken cancellationToken);

    Task<int> Create(Maintenance maintenance, CancellationToken cancellationToken);

    Task<int> Update(int id, int? carId, string? workType, string? description, decimal? cost,
        DateOnly? date, CancellationToken cancellationToken);

    Task<int> Delete(int id, CancellationToken cancellationToken);
}