using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class TripStatusRepository : ITripStatusRepository
{
    private readonly CarsharingDbContext _context;

    public TripStatusRepository(CarsharingDbContext context)
    {
        _context = context;
    }
    public async Task<List<TripStatus>> Get()
    {
        var tripStatusEntities = await _context.TripStatus
            .AsNoTracking()
            .ToListAsync();

        var tripStatuses = tripStatusEntities
            .Select(b => TripStatus.Create(
                b.Id,
                b.Name).tripStatus)
            .ToList();

        return tripStatuses;
    }

    public async Task<bool> Exists(int id)
    {
        return await _context.TripStatus
            .AsNoTracking()
            .AnyAsync(b => b.Id == id);
    }
}