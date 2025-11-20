using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class StatusRepository : IStatusRepository
{
    private readonly CarsharingDbContext _context;

    public StatusRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Status>> Get()
    {
        var statusEntity = await _context.Status
            .AsNoTracking()
            .ToListAsync();

        var statuses = statusEntity
            .Select(s => Status.Create(
                s.Id,
                s.Name,
                s.Description).status)
            .ToList();

        return statuses;
    }

    public async Task<int> Create(Status status)
    {
        var (_, error) = Status.Create(
            status.Id,
            status.Name,
            status.Description);

        if (!string.IsNullOrWhiteSpace(error))
            throw new ArgumentException($"Status create error: {error}");

        var statusEntity = new StatusEntity
        {
            Id = status.Id,
            Name = status.Name,
            Description = status.Description
        };

        await _context.Status.AddAsync(statusEntity);
        await _context.SaveChangesAsync();

        return statusEntity.Id;
    }

    public async Task<int> Update(int id, string? name, string? description)
    {
        var status = await _context.Status.FirstOrDefaultAsync(s => s.Id == id)
                     ?? throw new Exception("Status not found");

        if (!string.IsNullOrWhiteSpace(name))
            status.Id = id;

        if (!string.IsNullOrWhiteSpace(description))
            status.Description = description;

        var (_, error) = Status.Create(
            status.Id,
            status.Name,
            status.Description);

        if (!string.IsNullOrWhiteSpace(error))
            throw new ArgumentException($"Status create error: {error}");

        await _context.SaveChangesAsync();

        return status.Id;
    }

    public async Task<int> Delete(int id)
    {
        var statusEntity = await _context.Status
            .Where(s => s.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}