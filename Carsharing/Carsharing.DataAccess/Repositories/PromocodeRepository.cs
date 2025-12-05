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

    public async Task<List<Promocode>> Get()
    {
        var promocodeEntities = await _context.Promocode
            .AsNoTracking()
            .ToListAsync();

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

    public async Task<List<Promocode>> GetPaged(int page, int limit)
    {
        var promocodeEntities = await _context.Promocode
            .AsNoTracking()
            .OrderBy(pr => pr.Id)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

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

    public async Task<int> GetCount()
    {
        return await _context.Promocode.CountAsync();
    }

    public async Task<List<Promocode>> GetById(int? id)
    {
        var promocodeEntities = await _context.Promocode
            .Where(pr => pr.Id == id)
            .AsNoTracking()
            .ToListAsync();

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

    public async Task<List<Promocode>> GetActive()
    {
        var promocodeEntities = await _context.Promocode
            .Where(pr => pr.EndDate >= DateOnly.FromDateTime(DateTime.UtcNow))
            .AsNoTracking()
            .ToListAsync();

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

    public async Task<List<Promocode>> GetPagedActive(int page, int limit)
    {
        var promocodeEntities = await _context.Promocode
            .Where(pr => pr.EndDate >= DateOnly.FromDateTime(DateTime.UtcNow))
            .AsNoTracking()
            .OrderBy(pr => pr.Id)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

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

    public async Task<int> GetCountActive()
    {
        return await _context.Promocode.Where(pr => pr.EndDate >= DateOnly.FromDateTime(DateTime.UtcNow)).CountAsync();
    }

    public async Task<List<Promocode>> GetByCode(string code)
    {
        var promocodeEntities = await _context.Promocode
            .Where(pr => pr.Code == code)
            .AsNoTracking()
            .ToListAsync();

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

    public async Task<int> Create(Promocode promocode)
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

        await _context.Promocode.AddAsync(promocodeEntity);
        await _context.SaveChangesAsync();

        return promocode.Id;
    }

    public async Task<int> Update(int id, int? statusId, string? code, decimal? discount, DateOnly? startDate,
        DateOnly? endDate)
    {
        var promocode = await _context.Promocode.FirstOrDefaultAsync(pr => pr.Id == id)
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

        await _context.SaveChangesAsync();

        return promocode.Id;
    }

    public async Task<int> Delete(int id)
    {
        var promocodeEntity = await _context.Promocode
            .Where(pr => pr.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}