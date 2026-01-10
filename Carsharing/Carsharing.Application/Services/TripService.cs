using Carsharing.Application.Abstractions;
using Carsharing.Application.DTOs;
using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Enum;
using Carsharing.Core.Exceptions;
using Carsharing.Core.Models;
using Carsharing.DataAccess;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.Application.Services;

public class TripService : ITripService
{
    private readonly IClientRepository _clientRepository;
    private readonly CarsharingDbContext _context;
    private readonly ITripStatusRepository _statusRepository;
    private readonly ITripDetailRepository _tripDetailRepository;
    private readonly ITripRepository _tripRepository;
    private readonly ICarsService _carsService;
    private readonly IBookingRepository _bookingRepository;

    public TripService(ITripRepository tripRepository, ITripDetailRepository tripDetailRepository,
        ITripStatusRepository statusRepository, IClientRepository clientRepository, CarsharingDbContext context,
        ICarsService carsService, IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
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

    public async Task<(List<TripHistoryDto> Items, int TotalCount)> GetPagedHistoryByUserId(int userId, int page,
        int limit)
    {
        var clients = await _clientRepository.GetClientByUserId(userId);
        var client = clients.FirstOrDefault();
        if (client == null) return ([], 0);

        return await _tripRepository.GetHistoryByClientId(client.Id, page, limit);
    }

    public async Task<List<TripWithInfoDto>> GetTripWithInfo(int id)
    {
        return await _tripRepository.GetTripWithDetailsById(id);
    }

    public async Task<CurrentTripDto?> GetActiveTripByClientId(int userId)
    {
        var client = await _clientRepository.GetClientByUserId(userId);
        var clientId = client.Select(c => c.Id).FirstOrDefault();

        return await _tripRepository.GetActiveTripDtoByClientId(clientId);
    }

    public async Task<TripFinishResult> FinishTripAsync(FinishTripRequest request)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var trip = await _tripRepository.GetByIdWithDetails(request.TripId);

            if (trip is not { EndTime: null })
                throw new Exception("Поездка не найдена или уже завершена");

            var now = DateTime.UtcNow;
            var car = trip.Booking?.Car;

            trip.EndTime = now;
            var durationMinutes = (decimal)(now - trip.StartTime).TotalMinutes;
            trip.Duration = durationMinutes;

            trip.Distance = request.Distance;
            trip.StatusId = (int)TripStatusEnum.Finished;

            if (trip.Booking != null)
            {
                trip.Booking.StatusId = (int)BookingStatusEnum.Completed;
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
                car.StatusId = (int)CarStatusEnum.Available;
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
        var trip = await _tripRepository.GetByIdWithDetails(tripId);

        if (trip is not { EndTime: null })
            throw new Exception("Поездка не найдена или уже завершена.");

        trip.StatusId = (int)TripStatusEnum.Cancelled;

        trip.Duration = 0;
        trip.Distance = 0;

        if (trip.Booking != null)
        {
            trip.Booking.StatusId = (int)BookingStatusEnum.Cancelled;

            if (trip.Booking.Car != null)
            {
                trip.Booking.Car.StatusId = (int)CarStatusEnum.Available;
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> CreateTripAsync(TripCreateRequest request)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var (trip, error) = Trip.Create(
                0,
                request.BookingId,
                request.StatusId,
                request.TariffType,
                request.StartTime,
                request.EndTime,
                request.Duration,
                request.Distance);

            if (!string.IsNullOrWhiteSpace(error))
                throw new ArgumentException(error);

            var tripId = await _tripRepository.Create(trip);

            var (tripDetail, errorTripDetail) = TripDetail.Create(
                0,
                tripId,
                request.StartLocation,
                request.EndLocation,
                request.InsuranceActive,
                request.FuelUsed,
                request.Refueled);

            if (!string.IsNullOrWhiteSpace(errorTripDetail))
                throw new ArgumentException(errorTripDetail);

            await _tripDetailRepository.Create(tripDetail);

            await _carsService.MarkCarAsUnavailableAsync(request.CarId);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return tripId;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<int> UpdateTrip(int id, TripUpdateRequest request)
    {
        var tripId = await _tripRepository.Update(
            id,
            request.BookingId,
            request.StatusId,
            request.TariffType,
            request.StartTime,
            request.EndTime,
            request.Duration,
            request.Distance);

        var isFinished = request.StatusId == (int)TripStatusEnum.Finished;
        var isCancelled = request.StatusId == (int)TripStatusEnum.Cancelled;

        if (request.EndTime == null && !isFinished && !isCancelled)
            return tripId;

        var bookings = await _bookingRepository.GetById(request.BookingId);
        var booking = bookings.FirstOrDefault()
                      ?? throw new NotFoundException("Booking not found for this trip");

        await _carsService.MarkCarAsAvailableAsync(booking.CarId);

        return tripId;
    }

    public async Task<int> DeleteTrip(int id)
    {
        return await _tripRepository.Delete(id);
    }
}