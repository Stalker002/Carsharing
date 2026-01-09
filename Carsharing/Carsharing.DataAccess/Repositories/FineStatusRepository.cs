using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class FineStatusRepository : IFineStatusRepository
{
    private readonly CarsharingDbContext _context;

    public FineStatusRepository(CarsharingDbContext context)
    {
        _context = context;
    }
    public async Task<List<FineStatus>> Get()
    {
        var fineStatusEntities = await _context.FineStatus
            .AsNoTracking()
            .ToListAsync();

        var fineStatuses = fineStatusEntities
            .Select(b => FineStatus.Create(
                b.Id,
                b.Name).fineStatus)
            .ToList();

        return fineStatuses;
    }

    public async Task<bool> Exists(int id)
    {
        return await _context.FineStatus
            .AsNoTracking()
            .AnyAsync(b => b.Id == id);
    }
}