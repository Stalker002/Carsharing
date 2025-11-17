using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class TripRepository : ITripRepository
{
    private readonly CarsharingDbContext _context;

    public TripRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Trip>> Get()
    {
        var tripEntities = await _context.Trip
            .AsNoTracking()
            .ToListAsync();

        var trips = tripEntities
            .Select(t => Trip.Create(
                t.Id,
                t.BookingId,
                t.StatusId,
                t.TariffType,
                t.StartTime,
                t.EndTime,
                t.Duration,
                t.Distance).trip)
            .ToList();

        return trips;
    }

    public async Task<int> Create(Trip trip)
    {
        var (_, error) = Trip.Create(
            trip.Id,
            trip.BookingId,
            trip.StatusId,
            trip.TariffType,
            trip.StartTime,
            trip.EndTime,
            trip.Duration,
            trip.Distance);

        if (!string.IsNullOrEmpty(error))
            throw new ArgumentException($"Create exception trip: {error}");

        var tripEntity = new TripEntity
        {
            Id = trip.Id,
            BookingId = trip.BookingId,
            StatusId = trip.StatusId,
            TariffType = trip.TariffType,
            StartTime = trip.StartTime,
            EndTime = trip.EndTime,
            Duration = trip.Duration,
            Distance = trip.Distance
        };

        await _context.Trip.AddAsync(tripEntity);
        await _context.SaveChangesAsync();

        return tripEntity.Id;
    }

    public async Task<int> Update(int id, int? bookingId, int? statusId, string? tariffType, DateTime? startTime,
        DateTime? endTime, decimal? duration, decimal? distance)
    {
        var trip = await _context.Trip.FirstOrDefaultAsync(t => t.Id == id)
                   ?? throw new Exception("Trip not found");

        if (bookingId.HasValue)
            trip.BookingId = bookingId.Value;

        if (statusId.HasValue)
            trip.StatusId = statusId.Value;

        if (!string.IsNullOrWhiteSpace(tariffType))
            trip.TariffType = tariffType;

        if (startTime.HasValue)
            trip.StartTime = startTime.Value;

        if (endTime.HasValue)
            trip.EndTime = endTime.Value;

        if (duration.HasValue)
            trip.Duration = duration.Value;

        if (distance.HasValue)
            trip.Distance = distance.Value;

        await _context.SaveChangesAsync();

        return trip.Id;
    }

    public async Task<int> Delete(int id)
    {
        var tripEntity = await _context.Trip
            .Where(t => t.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}