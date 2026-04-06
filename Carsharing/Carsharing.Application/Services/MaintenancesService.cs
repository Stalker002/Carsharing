using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class MaintenancesService : IMaintenancesService
{
    private readonly IMaintenanceRepository _maintenanceRepository;

    public MaintenancesService(IMaintenanceRepository maintenanceRepository)
    {
        _maintenanceRepository = maintenanceRepository;
    }

    public async Task<List<Maintenance>> GetMaintenances(CancellationToken cancellationToken)
    {
        return await _maintenanceRepository.Get(cancellationToken);
    }

    public async Task<List<Maintenance>> GetMaintenanceById(int id, CancellationToken cancellationToken)
    {
        return await _maintenanceRepository.GetById(id, cancellationToken);
    }

    public async Task<List<Maintenance>> GetMaintenanceByCarId(int carId, CancellationToken cancellationToken)
    {
        return await _maintenanceRepository.GetByCarId(carId, cancellationToken);
    }

    public async Task<List<Maintenance>> GetByDateRange(DateOnly from, DateOnly to, CancellationToken cancellationToken)
    {
        return await _maintenanceRepository.GetByDateRange(from, to, cancellationToken);
    }

    public async Task<int> CreateMaintenance(Maintenance maintenance, CancellationToken cancellationToken)
    {
        return await _maintenanceRepository.Create(maintenance, cancellationToken);
    }

    public async Task<int> UpdateMaintenance(int id, int? carId, string? workType, string? description, decimal? cost,
        DateOnly? date, CancellationToken cancellationToken)
    {
        return await _maintenanceRepository.Update(id, carId, workType, description, cost, date, cancellationToken);
    }

    public async Task<int> DeleteMaintenance(int id, CancellationToken cancellationToken)
    {
        return await _maintenanceRepository.Delete(id, cancellationToken);
    }
}
