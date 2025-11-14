using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class InsuranceRepository
{
    private readonly CarsharingDbContext _context;

    public InsuranceRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Insurance>> Get()
    {
        var insuranceEntities = await _context.Insurance
            .AsNoTracking()
            .ToListAsync();

        var insurances = insuranceEntities
            .Select(i => Insurance.Create(
                i.Id,
                i.CarId,
                i.StatusId,
                i.Type,
                i.Company,
                i.PolicyNumber,
                i.StartDate,
                i.EndDate,
                i.Cost).insurance)
            .ToList();

        return insurances;
    }

    public async Task<int> Create(Insurance insurance)
    {
        var (_, error) = Insurance.Create(
            insurance.Id,
            insurance.CarId,
            insurance.StatusId,
            insurance.Type,
            insurance.Company,
            insurance.PolicyNumber,
            insurance.StartDate,
            insurance.EndDate,
            insurance.Cost);

        if (!string.IsNullOrWhiteSpace(error))
            throw new Exception($"Insurance create exception: {error}");

        var insuranceEntity = new InsuranceEntity()
        {
            Id = insurance.Id,
            CarId = insurance.CarId,
            StatusId = insurance.StatusId,
            Type = insurance.Type,
            Company = insurance.Company,
            PolicyNumber = insurance.PolicyNumber,
            StartDate = insurance.StartDate,
            EndDate = insurance.EndDate,
            Cost = insurance.Cost
        };

        await _context.Insurance.AddAsync(insuranceEntity);
        await _context.SaveChangesAsync();

        return insuranceEntity.Id;
    }

    public async Task<int> Update(int id, int? carId, int? statusId, string type, string? company, string? policyNumber,
        DateOnly? startDate, DateOnly? endDate, decimal? cost)
    {
        var insurance = await _context.Insurance.FirstOrDefaultAsync(i => i.Id == id)
                        ?? throw new Exception("Insurance not found");

        if (carId.HasValue)
            insurance.CarId = carId.Value;

        if (statusId.HasValue)
            insurance.StatusId = statusId.Value;
        
        if (!string.IsNullOrWhiteSpace(type))
            insurance.Type = type;

        if (!string.IsNullOrWhiteSpace(company))
            insurance.Company = company;

        if (!string.IsNullOrWhiteSpace(policyNumber))
            insurance.PolicyNumber = policyNumber;

        if (startDate.HasValue)
            insurance.StartDate = startDate.Value;

        if (endDate.HasValue)
            insurance.EndDate = endDate.Value;

        if (cost.HasValue)
            insurance.Cost = cost.Value;

        await _context.SaveChangesAsync();

        return insurance.Id;
    }

    public async Task<int> Delete(int id)
    {
        var insuranceEntity = await _context.Insurance
            .Where(i => i.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}