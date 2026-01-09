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
    public async Task<List<CarStatus>> Get()
    {
        var carStatusEntities = await _context.CarStatus
            .AsNoTracking()
            .ToListAsync();

        var carStatuses = carStatusEntities
            .Select(b => CarStatus.Create(
                b.Id,
                b.Name).carStatus)
            .ToList();

        return carStatuses;
    }

    public async Task<List<CarStatus>> GetById(int id)
    {
        var carStatusEntities = await _context.CarStatus
            .AsNoTracking()
            .Where(c => c.Id == id)
            .ToListAsync();

        var carStatuses = carStatusEntities
            .Select(b => CarStatus.Create(
                b.Id,
                b.Name).carStatus)
            .ToList();

        return carStatuses;
    }

    public async Task<bool> Exists(int id)
    {
        return await _context.CarStatus
            .AsNoTracking()
            .AnyAsync(b => b.Id == id);
    }
}