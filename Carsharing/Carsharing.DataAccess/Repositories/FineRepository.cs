using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class FineRepository : IFineRepository
{
    private readonly CarsharingDbContext _context;

    public FineRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Fine>> Get()
    {
        var fineEntities = await _context.Fine
            .AsNoTracking()
            .ToListAsync();

        var fines = fineEntities
            .Select(f => Fine.Create(
                f.Id,
                f.TripId,
                f.StatusId,
                f.Type,
                f.Amount,
                f.Date).fine)
            .ToList();

        return fines;
    }

    public async Task<List<Fine>> GetPaged(int page, int limit)
    {
        var fineEntities = await _context.Fine
            .AsNoTracking()
            .Skip((page - 1) * limit)
            .OrderBy(f => f.Id)
            .Take(limit)
            .ToListAsync();

        var fines = fineEntities
            .Select(f => Fine.Create(
                f.Id,
                f.TripId,
                f.StatusId,
                f.Type,
                f.Amount,
                f.Date).fine)
            .ToList();

        return fines;
    }

    public async Task<int> GetCount()
    {
        return await _context.Fine.CountAsync();
    }

    public async Task<List<Fine>> GetById(int id)
    {
        var fineEntities = await _context.Fine
            .Where(f => f.Id == id)
            .AsNoTracking()
            .ToListAsync();

        var fines = fineEntities
            .Select(f => Fine.Create(
                f.Id,
                f.TripId,
                f.StatusId,
                f.Type,
                f.Amount,
                f.Date).fine)
            .ToList();

        return fines;
    }

    public async Task<List<Fine>> GetByTripId(int tripId)
    {
        var fineEntities = await _context.Fine
            .Where(f => f.TripId == tripId)
            .OrderBy(f => f.Id)
            .AsNoTracking()
            .ToListAsync();

        var fines = fineEntities
            .Select(f => Fine.Create(
                f.Id,
                f.TripId,
                f.StatusId,
                f.Type,
                f.Amount,
                f.Date).fine)
            .ToList();

        return fines;
    }

    public async Task<List<Fine>> GetByStatusId(int statusId)
    {
        var fineEntities = await _context.Fine
            .Where(f => f.StatusId == statusId)
            .OrderBy(f => f.Id)
            .AsNoTracking()
            .ToListAsync();

        var fines = fineEntities
            .Select(f => Fine.Create(
                f.Id,
                f.TripId,
                f.StatusId,
                f.Type,
                f.Amount,
                f.Date).fine)
            .ToList();

        return fines;
    }

    public async Task<int> Create(Fine fine)
    {
        var (_, error) = Fine.Create(
            fine.Id,
            fine.TripId,
            fine.StatusId,
            fine.Type,
            fine.Amount,
            fine.Date);

        if (!string.IsNullOrWhiteSpace(error))
            throw new Exception($"Fine create exception: {error}");

        var fineEntity = new FineEntity
        {
            Id = fine.Id,
            TripId = fine.TripId,
            StatusId = fine.StatusId,
            Type = fine.Type,
            Amount = fine.Amount,
            Date = fine.Date
        };

        await _context.Fine.AddAsync(fineEntity);
        await _context.SaveChangesAsync();

        return fineEntity.Id;
    }

    public async Task<int> Update(int id, int? tripId, int? statusId, string? type, decimal? amount,
        DateTime? date)
    {
        var fine = await _context.Fine.FirstOrDefaultAsync(f => f.Id == id)
                   ?? throw new Exception("Fine not found");

        if (tripId.HasValue)
            fine.TripId = tripId.Value;

        if (statusId.HasValue)
            fine.StatusId = statusId.Value;

        if (!string.IsNullOrWhiteSpace(type))
            fine.Type = type;

        if (amount.HasValue)
            fine.Amount = amount.Value;

        if (date.HasValue)
            fine.Date = date.Value;

        var (_, error) = Fine.Create(
            fine.Id,
            fine.TripId,
            fine.StatusId,
            fine.Type,
            fine.Amount,
            fine.Date);

        if (!string.IsNullOrWhiteSpace(error))
            throw new Exception($"Fine create exception: {error}");

        await _context.SaveChangesAsync();

        return fine.Id;
    }

    public async Task<int> Delete(int id)
    {
        var fineEntity = await _context.Fine
            .Where(f => f.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}