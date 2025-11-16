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

    public async Task<List<Maintenance>> GetMaintenances()
    {
        return await _maintenanceRepository.Get();
    }

    public async Task<int> CreateMaintenance(Maintenance maintenance)
    {
        return await _maintenanceRepository.Create(maintenance);
    }

    public async Task<int> UpdateMaintenance(int id, int? carId, string? workType, string? description, decimal? cost,
        DateOnly? date)
    {
        return await _maintenanceRepository.Update(id, carId, workType, description, cost, date);
    }

    public async Task<int> DeleteMaintenance(int id)
    {
        return await _maintenanceRepository.Delete(id);
    }
}