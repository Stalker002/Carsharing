using Carsharing.Application.DTOs;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Enum;
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
            .OrderByDescending(t => t.StartTime)
            .ThenByDescending(t => t.Id)
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

    public async Task<List<Trip>> GetPaged(int page, int limit)
    {
        var tripEntities = await _context.Trip
            .AsNoTracking()
            .OrderByDescending(t => t.StartTime)
            .ThenByDescending(t => t.Id)
            .Skip((page - 1) * limit)
            .Take(limit)
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

    public async Task<int> GetCount()
    {
        return await _context.Trip.CountAsync();
    }

    public async Task<List<Trip>> GetById(int id)
    {
        var tripEntities = await _context.Trip
            .Where(tr => tr.Id == id)
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

    public async Task<List<Trip>> GetByBookingId(List<int> bookingIds)
    {
        var tripEntities = await _context.Trip
            .Where(tr => bookingIds.Contains(tr.BookingId))
            .OrderBy(tr => tr.Id)
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

    public async Task<int> GetCountByBooking(List<int> bookingIds)
    {
        return await _context.Trip.Where(tr => bookingIds.Contains(tr.BookingId)).CountAsync();
    }

    public async Task<List<TripWithInfoDto>> GetTripWithDetailsById(int id)
    {
        return await _context.Trip
            .AsNoTracking()
            .Where(t => t.Id == id)
            .Select(t => new TripWithInfoDto(
                t.Id,
                t.BookingId,
                t.StatusId,
                t.TripDetail != null ? t.TripDetail.StartLocation : null,
                t.TripDetail != null ? t.TripDetail.EndLocation : null,
                t.TripDetail != null && t.TripDetail.InsuranceActive,
                t.TripDetail != null ? t.TripDetail.FuelUsed : 0,
                t.TripDetail != null ? t.TripDetail.Refueled : 0,
                t.TariffType,
                t.StartTime,
                t.EndTime,
                t.Duration,
                t.Distance
            ))
            .ToListAsync();
    }

    public async Task<(List<TripHistoryDto> Items, int TotalCount)> GetHistoryByClientId(int clientId, int page, int limit)
    {
        var query = _context.Trip
            .AsNoTracking()
            .Where(t => t.Booking != null && t.Booking.ClientId == clientId && t.EndTime != null);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(t => t.StartTime)
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(t => new TripHistoryDto(
                t.Id,
                t.Booking!.Car!.SpecificationCar!.Brand,
                t.Booking.CarId,
                t.Booking.Car.SpecificationCar.Model,
                t.Booking.Car.ImagePath,

                (t.Bill != null && t.Bill.StatusId == (int)BillStatusEnum.Paid)
                    ? "Оплачено"
                    : t.TripStatus!.Name,

                t.StartTime,
                t.EndTime,
                t.Bill != null ? t.Bill.Amount : 0,
                t.TariffType,
                t.Duration,
                t.Distance,
                t.TripDetail != null && t.TripDetail.InsuranceActive,
                t.TripDetail != null ? t.TripDetail.FuelUsed : 0,
                t.TripDetail != null ? t.TripDetail.Refueled : 0,
                t.TripDetail != null ? t.TripDetail.StartLocation : "Неизвестно",
                t.TripDetail != null ? t.TripDetail.EndLocation : "Неизвестно"
            ))
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<CurrentTripDto?> GetActiveTripDtoByClientId(int clientId)
    {
        // Перенесли логику из сервиса GetActiveTripByClientId
        var tripEntity = await _context.Trip
            .AsNoTracking()
            .Include(t => t.Booking)
            .ThenInclude(b => b!.Car)
            .ThenInclude(c => c!.SpecificationCar)
            .Include(t => t.Booking)
            .ThenInclude(b => b!.Car)
            .ThenInclude(c => c!.Tariff)
            .Where(t => t.Booking!.ClientId == clientId &&
                        t.EndTime == null &&
                        (t.StatusId == (int)TripStatusEnum.WaitingStart || t.StatusId == (int)TripStatusEnum.EnRoute))
            .FirstOrDefaultAsync();

        if (tripEntity == null) return null;

        var car = tripEntity.Booking?.Car;

        return new CurrentTripDto
        (
            tripEntity.Id,
            tripEntity.StartTime,
            tripEntity.TariffType,
            car!.Id,
            car.SpecificationCar!.Brand,
            car.SpecificationCar!.Model,
            car.ImagePath,
            car.Location,
            car.Tariff!.PricePerMinute,
            car.Tariff.PricePerKm,
            car.Tariff.PricePerDay
        );
    }

    public async Task<TripEntity?> GetByIdWithDetails(int id)
    {
        return await _context.Trip
            .Include(t => t.Booking)
            .ThenInclude(b => b!.Car)
            .FirstOrDefaultAsync(t => t.Id == id);
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