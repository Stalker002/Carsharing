using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class PromocodeRepository : IPromocodeRepository
{
    private readonly CarsharingDbContext _context;

    public PromocodeRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Promocode>> Get(CancellationToken cancellationToken)
    {
        var promocodeEntities = await _context.Promocode
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var promocodes = promocodeEntities
            .Select(pr => Promocode.Create(
                pr.Id,
                pr.StatusId,
                pr.Code,
                pr.Discount,
                pr.StartDate,
                pr.EndDate).promocode)
            .ToList();

        return promocodes;
    }

    public async Task<List<Promocode>> GetPaged(int page, int limit, CancellationToken cancellationToken)
    {
        var promocodeEntities = await _context.Promocode
            .AsNoTracking()
            .OrderByDescending(p => p.StartDate)
            .ThenByDescending(p => p.Id)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync(cancellationToken);

        var promocodes = promocodeEntities
            .Select(pr => Promocode.Create(
                pr.Id,
                pr.StatusId,
                pr.Code,
                pr.Discount,
                pr.StartDate,
                pr.EndDate).promocode)
            .ToList();

        return promocodes;
    }

    public async Task<int> GetCount(CancellationToken cancellationToken)
    {
        return await _context.Promocode.CountAsync(cancellationToken);
    }

    public async Task<List<Promocode>> GetById(int? id, CancellationToken cancellationToken)
    {
        var promocodeEntities = await _context.Promocode
            .Where(pr => pr.Id == id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var promocodes = promocodeEntities
            .Select(pr => Promocode.Create(
                pr.Id,
                pr.StatusId,
                pr.Code,
                pr.Discount,
                pr.StartDate,
                pr.EndDate).promocode)
            .ToList();

        return promocodes;
    }

    public async Task<List<Promocode>> GetActive(CancellationToken cancellationToken)
    {
        var promocodeEntities = await _context.Promocode
            .Where(pr => pr.EndDate >= DateOnly.FromDateTime(DateTime.UtcNow))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var promocodes = promocodeEntities
            .Select(pr => Promocode.Create(
                pr.Id,
                pr.StatusId,
                pr.Code,
                pr.Discount,
                pr.StartDate,
                pr.EndDate).promocode)
            .ToList();

        return promocodes;
    }

    public async Task<List<Promocode>> GetPagedActive(int page, int limit, CancellationToken cancellationToken)
    {
        var promocodeEntities = await _context.Promocode
            .Where(pr => pr.EndDate >= DateOnly.FromDateTime(DateTime.UtcNow))
            .AsNoTracking()
            .OrderBy(pr => pr.Id)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync(cancellationToken);

        var promocodes = promocodeEntities
            .Select(pr => Promocode.Create(
                pr.Id,
                pr.StatusId,
                pr.Code,
                pr.Discount,
                pr.StartDate,
                pr.EndDate).promocode)
            .ToList();

        return promocodes;
    }

    public async Task<int> GetCountActive(CancellationToken cancellationToken)
    {
        return await _context.Promocode.Where(pr => pr.EndDate >= DateOnly.FromDateTime(DateTime.UtcNow))
            .CountAsync(cancellationToken);
    }

    public async Task<List<Promocode>> GetByCode(string code, CancellationToken cancellationToken)
    {
        var promocodeEntities = await _context.Promocode
            .Where(pr => pr.Code == code)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var promocodes = promocodeEntities
            .Select(pr => Promocode.Create(
                pr.Id,
                pr.StatusId,
                pr.Code,
                pr.Discount,
                pr.StartDate,
                pr.EndDate).promocode)
            .ToList();

        return promocodes;
    }

    public async Task<int> Create(Promocode promocode, CancellationToken cancellationToken)
    {
        var (_, error) = Promocode.Create(
            promocode.Id,
            promocode.StatusId,
            promocode.Code,
            promocode.Discount,
            promocode.StartDate,
            promocode.EndDate);

        if (!string.IsNullOrWhiteSpace(error))
            throw new Exception($"Promocode create exception: {error}");

        var promocodeEntity = new PromocodeEntity
        {
            Id = promocode.Id,
            StatusId = promocode.StatusId,
            Code = promocode.Code,
            Discount = promocode.Discount,
            StartDate = promocode.StartDate,
            EndDate = promocode.EndDate
        };

        await _context.Promocode.AddAsync(promocodeEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return promocode.Id;
    }

    public async Task<int> Update(int id, int? statusId, string? code, decimal? discount, DateOnly? startDate,
        DateOnly? endDate, CancellationToken cancellationToken)
    {
        var promocode = await _context.Promocode.FirstOrDefaultAsync(pr => pr.Id == id, cancellationToken)
                        ?? throw new Exception("Promocode not found");

        if (statusId.HasValue)
            promocode.StatusId = statusId.Value;

        if (!string.IsNullOrWhiteSpace(code))
            promocode.Code = code;

        if (discount.HasValue)
            promocode.Discount = discount.Value;

        if (startDate.HasValue)
            promocode.StartDate = startDate.Value;

        if (endDate.HasValue)
            promocode.EndDate = endDate.Value;

        var (_, error) = Promocode.Create(
            promocode.Id,
            promocode.StatusId,
            promocode.Code,
            promocode.Discount,
            promocode.StartDate,
            promocode.EndDate);

        if (!string.IsNullOrWhiteSpace(error))
            throw new Exception($"Promocode create exception: {error}");

        await _context.SaveChangesAsync(cancellationToken);

        return promocode.Id;
    }

    public async Task<int> Delete(int id, CancellationToken cancellationToken)
    {
        return await _context.Promocode
            .Where(pr => pr.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}