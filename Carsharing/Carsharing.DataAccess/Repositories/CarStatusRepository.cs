using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class CarStatusRepository : ICarStatusRepository
{
    private readonly CarsharingDbContext _context;

    public CarStatusRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<CarStatus>> Get(CancellationToken cancellationToken)
    {
        var carStatusEntities = await _context.CarStatus
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var carStatuses = carStatusEntities
            .Select(b => CarStatus.Create(
                b.Id,
                b.Name!).carStatus)
            .ToList();

        return carStatuses;
    }

    public async Task<List<CarStatus>> GetById(int id, CancellationToken cancellationToken)
    {
        var carStatusEntities = await _context.CarStatus
            .AsNoTracking()
            .Where(c => c.Id == id)
            .ToListAsync(cancellationToken);

        var carStatuses = carStatusEntities
            .Select(b => CarStatus.Create(
                b.Id,
                b.Name!).carStatus)
            .ToList();

        return carStatuses;
    }

    public async Task<bool> Exists(int id, CancellationToken cancellationToken)
    {
        return await _context.CarStatus
            .AsNoTracking()
            .AnyAsync(b => b.Id == id, cancellationToken);
    }
}