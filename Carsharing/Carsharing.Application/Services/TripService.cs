using Carsharing.Application.Abstractions;
using Carsharing.Application.DTOs;
using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Enum;
using Carsharing.Core.Exceptions;
using Carsharing.Core.Models;
using Carsharing.DataAccess;

namespace Carsharing.Application.Services;

public class TripService : ITripService
{
    private readonly IBillRepository _billRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly ICarsService _carsService;
    private readonly IClientRepository _clientRepository;
    private readonly CarsharingDbContext _context;
    private readonly ITripDetailRepository _tripDetailRepository;
    private readonly ITripRepository _tripRepository;

    private TripService(ITripRepository tripRepository, ITripDetailRepository tripDetailRepository, IClientRepository clientRepository, CarsharingDbContext context,
        ICarsService carsService, IBookingRepository bookingRepository, IBillRepository billRepository)
    {
        _billRepository = billRepository;
        _bookingRepository = bookingRepository;
        _carsService = carsService;
        _context = context;
        _clientRepository = clientRepository;
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
            var billId = await _tripRepository.FinishTripAsync(
                request.TripId,
                request.Distance,
                request.EndLocation,
                request.FuelLevel
            );

            await transaction.CommitAsync();

            var bill = await _billRepository.GetById(billId);

            return new TripFinishResult(billId, bill?.Amount, "Поездка успешно завершена");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> CancelTripAsync(int tripId)
    {
        await _tripRepository.CancelTripAsync(tripId);
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

            if (!string.IsNullOrWhiteSpace(error)) throw new ArgumentException(error);

            var tripId = await _tripRepository.Create(trip);

            var (tripDetail, errorTripDetail) = TripDetail.Create(
                0, tripId, 
                request.StartLocation,
                request.EndLocation,
                request.InsuranceActive, 
                request.FuelUsed, 
                request.Refueled);

            if (!string.IsNullOrWhiteSpace(errorTripDetail)) throw new ArgumentException(errorTripDetail);

            await _tripDetailRepository.Create(tripDetail);
            await _carsService.MarkCarAsUnavailableAsync(request.CarId);

            await transaction.CommitAsync();

            return tripId;
        }
        catch
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