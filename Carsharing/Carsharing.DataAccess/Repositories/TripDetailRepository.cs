using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class TripDetailRepository : ITripDetailRepository
{
    private readonly CarsharingDbContext _context;

    public TripDetailRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<TripDetail>> Get()
    {
        var detailEntities = await _context.TripDetail
            .AsNoTracking()
            .ToListAsync();

        var details = detailEntities
            .Select(d => TripDetail.Create(
                d.Id,
                d.TripId,
                d.StartLocation,
                d.EndLocation,
                d.InsuranceActive,
                d.FuelUsed,
                d.Refueled).tripDetail)
            .ToList();

        return details;
    }

    public async Task<List<TripDetail>> GetTrip()
    {
        var detailEntities = await _context.TripDetail
            .AsNoTracking()
            .ToListAsync();

        var details = detailEntities
            .Select(d => TripDetail.Create(
                d.Id,
                d.TripId,
                d.StartLocation,
                d.EndLocation,
                d.InsuranceActive,
                d.FuelUsed,
                d.Refueled).tripDetail)
            .ToList();

        return details;
    }

    public async Task<int> Create(TripDetail tripDetail)
    {
        var (_, error) = TripDetail.Create(
            tripDetail.Id,
            tripDetail.TripId,
            tripDetail.StartLocation,
            tripDetail.EndLocation,
            tripDetail.InsuranceActive,
            tripDetail.FuelUsed,
            tripDetail.Refueled);

        if (!string.IsNullOrWhiteSpace(error))
            throw new Exception($"Trip detail create exception: {error}");

        var tripDetailEntity = new TripDetailEntity
        {
            Id = tripDetail.Id,
            TripId = tripDetail.TripId,
            StartLocation = tripDetail.StartLocation,
            EndLocation = tripDetail.EndLocation,
            InsuranceActive = tripDetail.InsuranceActive,
            FuelUsed = tripDetail.FuelUsed,
            Refueled = tripDetail.Refueled
        };

        await _context.TripDetail.AddAsync(tripDetailEntity);
        await _context.SaveChangesAsync();

        return tripDetail.Id;
    }

    public async Task<int> Update(int id, int? tripId, string? startLocation, string? endLocation,
        bool? insuranceActive, decimal? fuelUsed, decimal? refueled)
    {
        var tripDetail = await _context.TripDetail.FirstOrDefaultAsync(d => d.Id == id)
                         ?? throw new Exception("Trip detail not found");

        if (tripId.HasValue)
            tripDetail.TripId = tripId.Value;

        if (!string.IsNullOrWhiteSpace(startLocation))
            tripDetail.StartLocation = startLocation;

        if (!string.IsNullOrWhiteSpace(endLocation))
            tripDetail.EndLocation = endLocation;

        if (insuranceActive.HasValue)
            tripDetail.InsuranceActive = insuranceActive.Value;

        if (fuelUsed.HasValue)
            tripDetail.FuelUsed = fuelUsed.Value;

        if (refueled.HasValue)
            tripDetail.Refueled = refueled.Value;

        var (_, error) = TripDetail.Create(
            tripDetail.Id,
            tripDetail.TripId,
            tripDetail.StartLocation,
            tripDetail.EndLocation,
            tripDetail.InsuranceActive,
            tripDetail.FuelUsed,
            tripDetail.Refueled);

        if (!string.IsNullOrWhiteSpace(error))
            throw new Exception($"Trip detail create exception: {error}");

        await _context.SaveChangesAsync();

        return tripDetail.Id;
    }

    public async Task<int> Delete(int id)
    {
        var tripDetailEntity = await _context.TripDetail
            .Where(d => d.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}