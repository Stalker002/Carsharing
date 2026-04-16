using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class BillStatusRepository(CarsharingDbContext context) : IBillStatusRepository
{
    public async Task<List<BillStatus>> Get(CancellationToken cancellationToken)
    {
        var billStatusEntities = await context.BillStatus
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var billStatuses = billStatusEntities
            .Select(b => BillStatus.Create(
                b.Id,
                b.Name!).billStatus)
            .ToList();

        return billStatuses;
    }

    public async Task<bool> Exists(int id, CancellationToken cancellationToken)
    {
        return await context.BillStatus
            .AsNoTracking()
            .AnyAsync(b => b.Id == id, cancellationToken);
    }
}