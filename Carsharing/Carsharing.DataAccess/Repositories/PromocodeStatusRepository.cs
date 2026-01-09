using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class PromocodeStatusRepository : IPromocodeStatusRepository
{
    private readonly CarsharingDbContext _context;

    public PromocodeStatusRepository(CarsharingDbContext context)
    {
        _context = context;
    }
    public async Task<List<PromocodeStatus>> Get()
    {
        var promocodeStatusEntities = await _context.PromocodeStatus
            .AsNoTracking()
            .ToListAsync();

        var promocodeStatuses = promocodeStatusEntities
            .Select(b => PromocodeStatus.Create(
                b.Id,
                b.Name).promocodeStatus)
            .ToList();

        return promocodeStatuses;
    }

    public async Task<bool> Exists(int id)
    {
        return await _context.PromocodeStatus
            .AsNoTracking()
            .AnyAsync(b => b.Id == id);
    }
}