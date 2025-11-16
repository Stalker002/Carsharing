using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class MaintenanceRepository : IMaintenanceRepository
{
    private readonly CarsharingDbContext _context;

    public MaintenanceRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Maintenance>> Get()
    {
        var maintenanceEntity = await _context.Maintenance
            .AsNoTracking()
            .ToListAsync();

        var maintenances = maintenanceEntity
            .Select(m => Maintenance.Create(
                m.Id,
                m.CarId,
                m.WorkType,
                m.Description,
                m.Cost,
                m.Date).maintenance)
            .ToList();

        return maintenances;
    }

    public async Task<int> Create(Maintenance maintenance)
    {
        var (_, error) = Maintenance.Create(
            maintenance.Id,
            maintenance.CarId,
            maintenance.WorkType,
            maintenance.Description,
            maintenance.Cost,
            maintenance.Date);

        if (!string.IsNullOrWhiteSpace(error))
            throw new ArgumentException($"Maintenance create error: {error}");

        var maintenanceEntity = new MaintenanceEntity()
        {
            Id = maintenance.Id,
            CarId = maintenance.CarId,
            WorkType = maintenance.WorkType,
            Description = maintenance.Description,
            Cost = maintenance.Cost,
            Date = maintenance.Date
        };

        await _context.Maintenance.AddAsync(maintenanceEntity);
        await _context.SaveChangesAsync();

        return maintenanceEntity.Id;
    }

    public async Task<int> Update(int id, int? carId, string? workType, string? description, decimal? cost,
        DateOnly? date)
    {
        var maintenance = await _context.Maintenance.FirstOrDefaultAsync(m => m.Id == id)
                          ?? throw new Exception("Maintenance not found");

        if (carId.HasValue)
            maintenance.CarId = carId.Value;

        if (!string.IsNullOrWhiteSpace(workType))
            maintenance.WorkType = workType;

        if (!string.IsNullOrWhiteSpace(description))
            maintenance.Description = description;

        if (cost.HasValue)
            maintenance.Cost = cost.Value;

        if (date.HasValue)
            maintenance.Date = date.Value;

        await _context.SaveChangesAsync();

        return maintenance.Id;
    }

    public async Task<int> Delete(int id)
    {
        var maintenanceEntity = await _context.Maintenance
            .Where(m => m.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}