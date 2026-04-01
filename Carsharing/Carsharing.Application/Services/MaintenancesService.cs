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

    public async Task<List<Maintenance>> GetMaintenances()
    {
        return await _maintenanceRepository.Get();
    }

    public async Task<List<Maintenance>> GetMaintenanceById(int id)
    {
        return await _maintenanceRepository.GetById(id);
    }

    public async Task<List<Maintenance>> GetMaintenanceByCarId(int carId)
    {
        return await _maintenanceRepository.GetByCarId(carId);
    }

    public async Task<List<Maintenance>> GetByDateRange(DateOnly from, DateOnly to)
    {
        return await _maintenanceRepository.GetByDateRange(from, to);
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