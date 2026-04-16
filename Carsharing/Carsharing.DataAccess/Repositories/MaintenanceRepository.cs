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

    public async Task<List<Maintenance>> Get(CancellationToken cancellationToken)
    {
        var maintenanceEntity = await _context.Maintenance
            .AsNoTracking()
            .ToListAsync(cancellationToken);

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

    public async Task<List<Maintenance>> GetById(int id, CancellationToken cancellationToken)
    {
        var maintenanceEntity = await _context.Maintenance
            .Where(m => m.Id == id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

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

    public async Task<List<Maintenance>> GetByCarId(int carId, CancellationToken cancellationToken)
    {
        var maintenanceEntity = await _context.Maintenance
            .Where(m => m.CarId == carId)
            .OrderByDescending(m => m.Date)
            .ThenByDescending(m => m.Id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

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

    public async Task<List<Maintenance>> GetByDateRange(DateOnly from, DateOnly to, CancellationToken cancellationToken)
    {
        var maintenanceEntity = await _context.Maintenance
            .Where(m => m.Date >= from && m.Date <= to)
            .OrderBy(m => m.Id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

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

    public async Task<int> Create(Maintenance maintenance, CancellationToken cancellationToken)
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

        var maintenanceEntity = new MaintenanceEntity
        {
            Id = maintenance.Id,
            CarId = maintenance.CarId,
            WorkType = maintenance.WorkType,
            Description = maintenance.Description,
            Cost = maintenance.Cost,
            Date = maintenance.Date
        };

        await _context.Maintenance.AddAsync(maintenanceEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return maintenanceEntity.Id;
    }

    public async Task<int> Update(int id, int? carId, string? workType, string? description, decimal? cost,
        DateOnly? date, CancellationToken cancellationToken)
    {
        var maintenance = await _context.Maintenance.FirstOrDefaultAsync(m => m.Id == id, cancellationToken)
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

        var (_, error) = Maintenance.Create(
            maintenance.Id,
            maintenance.CarId,
            maintenance.WorkType,
            maintenance.Description,
            maintenance.Cost,
            maintenance.Date);

        if (!string.IsNullOrWhiteSpace(error))
            throw new ArgumentException($"Maintenance create error: {error}");

        await _context.SaveChangesAsync(cancellationToken);

        return maintenance.Id;
    }

    public async Task<int> Delete(int id, CancellationToken cancellationToken)
    {
        return await _context.Maintenance
            .Where(m => m.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}