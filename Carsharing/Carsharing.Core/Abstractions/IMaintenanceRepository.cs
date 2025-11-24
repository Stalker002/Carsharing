using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IMaintenanceRepository
{
    Task<List<Maintenance>> Get();
    Task<List<Maintenance>> GetById(int id);
    Task<List<Maintenance>> GetByCarId(int carId);
    Task<List<Maintenance>> GetByDateRange(DateOnly from, DateOnly to);
    Task<int> Create(Maintenance maintenance);

    Task<int> Update(int id, int? carId, string? workType, string? description, decimal? cost,
        DateOnly? date);

    Task<int> Delete(int id);
}