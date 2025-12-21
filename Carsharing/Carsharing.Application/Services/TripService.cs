using Carsharing.Application.DTOs;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.Application.Services;

public class TripService : ITripService
{
    private readonly IClientRepository _clientRepository;
    private readonly CarsharingDbContext _context;
    private readonly IStatusRepository _statusRepository;
    private readonly ITripDetailRepository _tripDetailRepository;
    private readonly ITripRepository _tripRepository;
    private readonly ICarsService _carsService;

    public TripService(ITripRepository tripRepository, ITripDetailRepository tripDetailRepository,
        IStatusRepository statusRepository, IClientRepository clientRepository, CarsharingDbContext context, ICarsService carsService)
    {
        _carsService = carsService;
        _context = context;
        _clientRepository = clientRepository;
        _statusRepository = statusRepository;
        _tripDetailRepository = tripDetailRepository;
        _tripRepository = tripRepository;
    }

    public async Task<List<Trip>> GetTrips()
    {
        return await _tripRepository.Get();
    }

    public async Task<List<Trip>> GetPagedTrips(int page, int limit)
    {
        return await _tripRepository.GetPaged(page, limit);
    }

    public async Task<int> GetCountTrips()
    {
        return await _tripRepository.GetCount();
    }

    public async Task<(List<TripHistoryDto> Items, int TotalCount)> GetPagedHistoryByClientId(int clientId, int page,
        int limit)
    {
        var query = from t in _context.Trip
                    join b in _context.Booking on t.BookingId equals b.Id
                    join c in _context.Car on b.CarId equals c.Id
                    join s in _context.SpecificationCar on c.SpecificationId equals s.Id
                    join st in _context.Status on t.StatusId equals st.Id
                    join td in _context.TripDetail on t.Id equals td.TripId into details
                    from td in details.DefaultIfEmpty()
                    join bill in _context.Bill on t.Id equals bill.TripId into bills
                    from bill in bills.DefaultIfEmpty()
                    where b.ClientId == clientId && t.EndTime != null
                    select new
                    {
                        t,
                        s,
                        c,
                        st,
                        bill,
                        td
                    };

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(x => x.t.StartTime)
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(x => new TripHistoryDto
            (
                x.t.Id,
                x.s.Brand,
                x.s.Model,
                x.c.ImagePath,
                x.bill != null && x.bill.StatusId == 7 ? "Оплачено" : x.st.Name,
                x.t.StartTime,
                x.t.EndTime,
                (decimal)(x.bill != null ? x.bill.Amount : 0)!,
                x.t.TariffType,
                x.t.Duration,
                x.t.Distance,
                x.td.InsuranceActive,
                x.td.FuelUsed,
                x.td.Refueled,
                x.td != null ? x.td.StartLocation : "Неизвестно",
                x.td != null ? x.td.EndLocation : "Неизвестно"
            ))
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<List<TripWithInfoDto>> GetTripWithInfo(int id)
    {
        var trip = await _tripRepository.GetById(id);
        var tripId = trip.Select(tr => tr.Id).ToList();

        var tripDetail = await _tripDetailRepository.GetByTripId(tripId);

        var response = (from d in tripDetail
                        join tr in trip on d.TripId equals tr.Id
                        select new TripWithInfoDto(
                            tr.Id,
                            tr.BookingId,
                            tr.StatusId,
                            d.StartLocation,
                            d.EndLocation,
                            d.InsuranceActive,
                            d.FuelUsed,
                            d.Refueled,
                            tr.TariffType,
                            tr.StartTime,
                            tr.EndTime,
                            tr.Duration,
                            tr.Distance)).ToList();

        return response;
    }

    public async Task<CurrentTripDto?> GetActiveTripByClientId(int userId)
    {
        var client = await _clientRepository.GetClientByUserId(userId);
        var clientId = client.Select(c => c.Id).FirstOrDefault();

        var tripEntity = await _context.Trip
            .AsNoTracking()
            .Include(t => t.Booking)
            .ThenInclude(b => b!.Car)
            .ThenInclude(c => c!.SpecificationCar)
            .Include(t => t.Booking)
            .ThenInclude(b => b!.Car)
            .ThenInclude(c => c!.Tariff)
            .Where(t => (t.Booking!.ClientId == clientId && t.EndTime == null && (t.StatusId == 8 || t.StatusId == 9)))
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
            car.SpecificationCar.Model,
            car.ImagePath,
            car.Location,
            car.Tariff!.PricePerMinute,
            car.Tariff.PricePerKm,
            car.Tariff.PricePerDay
        );
    }

    public async Task<TripFinishResult> FinishTripAsync(FinishTripRequest request)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var trip = await _context.Trip
                .Include(t => t.Booking)
                .ThenInclude(b => b!.Car)
                .FirstOrDefaultAsync(t => t.Id == request.TripId);

            if (trip is not { EndTime: null })
                throw new Exception("Поездка не найдена или уже завершена");

            var now = DateTime.UtcNow;
            var car = trip.Booking?.Car;

            trip.EndTime = now;
            var durationMinutes = (decimal)(now - trip.StartTime).TotalMinutes;
            trip.Duration = durationMinutes;

            trip.Distance = request.Distance;
            trip.StatusId = 10;

            if (trip.Booking != null)
            {
                trip.Booking.StatusId = 6;
            }

            var tripDetail = await _context.TripDetail
                                .FirstOrDefaultAsync(td => td.TripId == trip.Id);

            if (tripDetail == null)
            {
                tripDetail = new TripDetailEntity { TripId = trip.Id, StartLocation = car?.Location ?? "Unknown" };
                _context.TripDetail.Add(tripDetail);
            }

            tripDetail.EndLocation = request.EndLocation;
            if (car != null)
            {
                var fuelDiff = car.FuelLevel - request.FuelLevel;



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
                car.StatusId = 1;
                car.Location = request.EndLocation;
                car.FuelLevel = request.FuelLevel;
            }

            await _context.SaveChangesAsync();

            var bill = await _context.Bill
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.TripId == trip.Id);

            if (bill == null) throw new Exception("Ошибка системы: Счет не был сформирован базой данных.");

            await transaction.CommitAsync();

            return new TripFinishResult(
                bill.Id,
                bill.Amount,
                "Поездка успешно завершена"
            );
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> CancelTripAsync(int tripId)
    {
        var trip = await _context.Trip
            .Include(t => t.Booking)
            .ThenInclude(b => b!.Car)
            .FirstOrDefaultAsync(t => t.Id == tripId);

        if (trip is not { EndTime: null })
            throw new Exception("Поездка не найдена или уже завершена.");

        trip.StatusId = 11;

        trip.Duration = 0;
        trip.Distance = 0;

        if (trip.Booking != null)
        {
            trip.Booking.StatusId = 7;

            if (trip.Booking.Car != null)
            {
                trip.Booking.Car.StatusId = 1;
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> CreateTrip(Trip trip)
    {
        return await _tripRepository.Create(trip);
    }

    public async Task<int> UpdateTrip(int id, int? bookingId, int? statusId, string? tariffType, DateTime? startTime,
        DateTime? endTime, decimal? duration, decimal? distance)
    {
        return await _tripRepository.Update(id, bookingId, statusId, tariffType, startTime, endTime, duration, distance);
    }

    public async Task<int> DeleteTrip(int id)
    {
        return await _tripRepository.Delete(id);
    }
}