using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class InsuranceStatusRepository : IInsuranceStatusRepository
{
    private readonly CarsharingDbContext _context;

    public InsuranceStatusRepository(CarsharingDbContext context)
    {
        _context = context;
    }
    public async Task<List<InsuranceStatus>> Get()
    {
        var insuranceStatusEntities = await _context.InsuranceStatus
            .AsNoTracking()
            .ToListAsync();

        var insuranceStatuses = insuranceStatusEntities
            .Select(b => InsuranceStatus.Create(
                b.Id,
                b.Name).insuranceStatus)
            .ToList();

        return insuranceStatuses;
    }

    public async Task<bool> Exists(int id)
    {
        return await _context.InsuranceStatus
            .AsNoTracking()
            .AnyAsync(b => b.Id == id);
    }
}