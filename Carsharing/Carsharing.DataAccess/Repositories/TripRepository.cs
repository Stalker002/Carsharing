using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Shared.Contracts.Trip;
using Shared.Enums;

namespace Carsharing.DataAccess.Repositories;

public class TripRepository : ITripRepository
{
    private readonly CarsharingDbContext _context;

    public TripRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Trip>> Get(CancellationToken cancellationToken)
    {
        var tripEntities = await _context.Trip
            .AsNoTracking()
            .OrderByDescending(t => t.StartTime)
            .ThenByDescending(t => t.Id)
            .ToListAsync(cancellationToken);

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

    public async Task<List<Trip>> GetPaged(int page, int limit, CancellationToken cancellationToken)
    {
        var tripEntities = await _context.Trip
            .AsNoTracking()
            .OrderByDescending(t => t.StartTime)
            .ThenByDescending(t => t.Id)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync(cancellationToken);

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

    public async Task<int> GetCount(CancellationToken cancellationToken)
    {
        return await _context.Trip.CountAsync(cancellationToken);
    }

    public async Task<List<Trip>> GetById(int id, CancellationToken cancellationToken)
    {
        var tripEntities = await _context.Trip
            .Where(tr => tr.Id == id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

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

    public async Task<List<Trip>> GetByBookingId(List<int> bookingIds, CancellationToken cancellationToken)
    {
        var tripEntities = await _context.Trip
            .Where(tr => bookingIds.Contains(tr.BookingId))
            .OrderBy(tr => tr.Id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

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

    public async Task<int> GetCountByBooking(List<int> bookingIds, CancellationToken cancellationToken)
    {
        return await _context.Trip.Where(tr => bookingIds.Contains(tr.BookingId)).CountAsync(cancellationToken);
    }

    public async Task<List<TripWithInfoDto>> GetTripWithDetailsById(int id, CancellationToken cancellationToken)
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
            .ToListAsync(cancellationToken);
    }

    public async Task<(List<TripHistoryDto> Items, int TotalCount)> GetHistoryByClientId(int clientId, int page,
        int limit, CancellationToken cancellationToken)
    {
        var query = _context.Trip
            .AsNoTracking()
            .Where(t => t.Booking != null && t.Booking.ClientId == clientId && t.EndTime != null);

        var totalCount = await query.CountAsync(cancellationToken);

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
                t.Bill != null && t.Bill.StatusId == (int)BillStatusEnum.Paid ? "Оплачено" : t.TripStatus!.Name!,
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
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<CurrentTripDto?> GetActiveTripDtoByClientId(int clientId, CancellationToken cancellationToken)
    {
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
            .FirstOrDefaultAsync(cancellationToken);

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
            car.Coordinates?.Y,
            car.Coordinates?.X,
            car.Tariff!.PricePerMinute,
            car.Tariff.PricePerKm,
            car.Tariff.PricePerDay,
            tripEntity.Distance.GetValueOrDefault(),
            car.FuelLevel
        );
    }

    public async Task UpdateTripLocationAsync(int tripId, string location, double latitude, double longitude,
        CancellationToken cancellationToken)
    {
        var trip = await _context.Trip
            .Include(t => t.Booking)
            .ThenInclude(b => b!.Car)
            .FirstOrDefaultAsync(t => t.Id == tripId, cancellationToken);

        if (trip is not { EndTime: null })
            throw new Exception("Поездка не найдена или уже завершена");

        var car = trip.Booking?.Car ?? throw new Exception("Автомобиль для поездки не найден");

        car.Location = location;
        car.Coordinates = new Point(longitude, latitude) { SRID = 4326 };

        if (trip.StatusId == (int)TripStatusEnum.WaitingStart)
            trip.StatusId = (int)TripStatusEnum.EnRoute;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> FinishTripAsync(int tripId, decimal distance, string endLocation, decimal fuelLevel,
        double carLatitude, double carLongitude, CancellationToken cancellationToken)
    {
        var trip = await _context.Trip
            .Include(t => t.Booking)
            .ThenInclude(b => b!.Car)
            .FirstOrDefaultAsync(t => t.Id == tripId, cancellationToken);

        if (trip is not { EndTime: null })
            throw new Exception("Поездка не найдена или уже завершена");

        var now = DateTime.UtcNow;
        var car = trip.Booking?.Car;

        trip.EndTime = now;
        trip.Duration = (decimal)(now - trip.StartTime).TotalMinutes;
        trip.Distance = distance;
        trip.StatusId = (int)TripStatusEnum.Finished;

        if (trip.Booking != null) trip.Booking.StatusId = (int)BookingStatusEnum.Completed;

        var tripDetail = await _context.TripDetail.FirstOrDefaultAsync(td => td.TripId == trip.Id, cancellationToken);

        if (tripDetail == null)
        {
            tripDetail = new TripDetailEntity { TripId = trip.Id, StartLocation = car?.Location ?? "Unknown" };
            _context.TripDetail.Add(tripDetail);
        }

        tripDetail.EndLocation = endLocation;

        if (car != null)
        {
            var fuelDiff = car.FuelLevel - fuelLevel;
            if (fuelDiff >= 0)
            {
                tripDetail.FuelUsed = fuelDiff;
                tripDetail.Refueled = 0;
            }
            else
            {
                tripDetail.FuelUsed = 0;
                tripDetail.Refueled = Math.Abs(fuelDiff);
            }

            car.StatusId = (int)CarStatusEnum.Available;
            car.Location = endLocation;
            car.Coordinates = new Point(carLongitude, carLatitude) { SRID = 4326 };
            car.FuelLevel = fuelLevel;
        }

        await _context.SaveChangesAsync(cancellationToken);

        // Пытаемся найти ID счета (для возврата), если он был создан триггером или другой логикой
        var billId = await _context.Bill
            .Where(b => b.TripId == trip.Id)
            .Select(b => b.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return billId;
    }

    public async Task CancelTripAsync(int tripId, CancellationToken cancellationToken)
    {
        var trip = await _context.Trip
            .Include(t => t.Booking)
            .ThenInclude(b => b!.Car)
            .FirstOrDefaultAsync(t => t.Id == tripId, cancellationToken);

        if (trip == null || trip.EndTime != null)
            throw new Exception("Поездка не найдена или уже завершена.");

        trip.StatusId = (int)TripStatusEnum.Cancelled;
        trip.Duration = 0;
        trip.Distance = 0;

        if (trip.Booking != null)
        {
            trip.Booking.StatusId = (int)BookingStatusEnum.Cancelled;
            if (trip.Booking.Car != null) trip.Booking.Car.StatusId = (int)CarStatusEnum.Available;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> Create(Trip trip, CancellationToken cancellationToken)
    {
        var calculatedDuration = trip.Duration;
        if (trip.StartTime != default && trip.EndTime.HasValue)
            calculatedDuration = decimal.Round((decimal)(trip.EndTime.Value - trip.StartTime).TotalMinutes, 2,
                MidpointRounding.AwayFromZero);

        var (_, error) = Trip.Create(
            trip.Id,
            trip.BookingId,
            trip.StatusId,
            trip.TariffType,
            trip.StartTime,
            trip.EndTime,
            calculatedDuration,
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
            Duration = calculatedDuration,
            Distance = trip.Distance
        };

        await _context.Trip.AddAsync(tripEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return tripEntity.Id;
    }

    public async Task<int> Update(int id, int? bookingId, int? statusId, string? tariffType, DateTime? startTime,
        DateTime? endTime, decimal? duration, decimal? distance, CancellationToken cancellationToken)
    {
        var trip = await _context.Trip.FirstOrDefaultAsync(t => t.Id == id, cancellationToken)
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

        if (trip.StartTime != default && trip.EndTime.HasValue)
        {
            trip.Duration = decimal.Round((decimal)(trip.EndTime.Value - trip.StartTime).TotalMinutes, 2,
                MidpointRounding.AwayFromZero);
        }
        else if (duration.HasValue)
        {
            trip.Duration = duration.Value;
        }

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

        await _context.SaveChangesAsync(cancellationToken);

        return trip.Id;
    }

    public async Task<int> Delete(int id, CancellationToken cancellationToken)
    {
        return await _context.Trip
            .Where(t => t.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
