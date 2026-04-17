using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class TariffRepository : ITariffRepository
{
    private readonly CarsharingDbContext _context;

    public TariffRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Tariff>> Get(CancellationToken cancellationToken)
    {
        var tariffEntities = await _context.Tariff
            .AsNoTracking()
            .OrderBy(t => t.Id)
            .ToListAsync(cancellationToken);

        var tariffs = tariffEntities
            .Select(t => Tariff.Create(
                t.Id,
                t.Name,
                t.PricePerMinute,
                t.PricePerKm,
                t.PricePerDay).tariff)
            .ToList();

        return tariffs;
    }

    public async Task<List<Tariff>> GetById(int id, CancellationToken cancellationToken)
    {
        var tariffEntities = await _context.Tariff
            .Where(t => t.Id == id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var tariffs = tariffEntities
            .Select(t => Tariff.Create(
                t.Id,
                t.Name,
                t.PricePerMinute,
                t.PricePerKm,
                t.PricePerDay).tariff)
            .ToList();

        return tariffs;
    }

    public async Task<int> Create(Tariff tariff, CancellationToken cancellationToken)
    {
        var (_, error) = Tariff.Create(
            tariff.Id,
            tariff.Name,
            tariff.PricePerMinute,
            tariff.PricePerKm,
            tariff.PricePerDay);

        if (!string.IsNullOrWhiteSpace(error))
            throw new ArgumentException($"Tariff create exception error: {error}");

        var tariffEntity = new TariffEntity
        {
            Id = tariff.Id,
            Name = tariff.Name,
            PricePerMinute = tariff.PricePerMinute,
            PricePerKm = tariff.PricePerKm,
            PricePerDay = tariff.PricePerDay
        };

        await _context.Tariff.AddAsync(tariffEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return tariffEntity.Id;
    }

    public async Task<int> Update(int id, string? name, decimal? pricePerMinute, decimal? pricePerKm,
        decimal? pricePerDay, CancellationToken cancellationToken)
    {
        var tariff = await _context.Tariff.FirstOrDefaultAsync(t => t.Id == id, cancellationToken)
                     ?? throw new Exception("Tariff not found");

        if (!string.IsNullOrWhiteSpace(name))
            tariff.Name = name;

        if (pricePerMinute.HasValue)
            tariff.PricePerMinute = pricePerMinute.Value;

        if (pricePerKm.HasValue)
            tariff.PricePerMinute = pricePerKm.Value;

        if (pricePerDay.HasValue)
            tariff.PricePerDay = pricePerDay.Value;

        var (_, error) = Tariff.Create(
            tariff.Id,
            tariff.Name,
            tariff.PricePerMinute,
            tariff.PricePerKm,
            tariff.PricePerDay);

        if (!string.IsNullOrWhiteSpace(error))
            throw new ArgumentException($"Tariff create exception error: {error}");

        await _context.SaveChangesAsync(cancellationToken);

        return tariff.Id;
    }

    public async Task<int> Delete(int id, CancellationToken cancellationToken)
    {
        return await _context.Tariff
            .Where(t => t.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}