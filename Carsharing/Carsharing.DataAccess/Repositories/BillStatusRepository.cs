using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class BillStatusRepository : IBillStatusRepository
{
    private readonly CarsharingDbContext _context;

    public BillStatusRepository(CarsharingDbContext context)
    {
        _context = context;
    }
    public async Task<List<BillStatus>> Get()
    {
        var billStatusEntities = await _context.BillStatus
            .AsNoTracking()
            .ToListAsync();

        var billStatuses = billStatusEntities
            .Select(b => BillStatus.Create(
                b.Id,
                b.Name).billStatus)
            .ToList();

        return billStatuses;
    }

    public async Task<bool> Exists(int id)
    {
        return await _context.BillStatus
            .AsNoTracking()
            .AnyAsync(b => b.Id == id);
    }
}