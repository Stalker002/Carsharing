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

    public async Task<List<TripDetail>> Get(CancellationToken cancellationToken)
    {
        var detailEntities = await _context.TripDetail
            .OrderBy(tr => tr.Id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

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

    public async Task<int> GetCarIdByTripId(int tripId, CancellationToken cancellationToken)
    {
        var carId = await _context.Trip
            .AsNoTracking()
            .Where(t => t.Id == tripId)
            .Select(t => t.Booking!.CarId)
            .FirstOrDefaultAsync(cancellationToken);

        return carId;
    }

    public async Task<int> Create(TripDetail tripDetail, CancellationToken cancellationToken)
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

        await _context.TripDetail.AddAsync(tripDetailEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return tripDetail.Id;
    }

    public async Task<int> Update(int id, int? tripId, string? startLocation, string? endLocation,
        bool? insuranceActive, decimal? fuelUsed, decimal? refueled, CancellationToken cancellationToken)
    {
        var tripDetail = await _context.TripDetail.FirstOrDefaultAsync(d => d.Id == id, cancellationToken)
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

        await _context.SaveChangesAsync(cancellationToken);

        return tripDetail.Id;
    }

    public async Task<int> Delete(int id, CancellationToken cancellationToken)
    {
        return await _context.TripDetail
            .Where(d => d.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}