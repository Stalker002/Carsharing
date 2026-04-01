using Carsharing.Application.Abstractions;
using Carsharing.Application.DTOs;
using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Enum;
using Carsharing.Core.Exceptions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class TripService(
    ITripRepository tripRepository,
    ITripDetailRepository tripDetailRepository,
    IClientRepository clientRepository,
    IUnitOfWork unitOfWork,
    ICarsService carsService,
    IBookingRepository bookingRepository,
    IBillRepository billRepository)
    : ITripService
{
    public async Task<List<Trip>> GetTrips()
    {
        return await tripRepository.Get();
    }

    public async Task<List<Trip>> GetPagedTrips(int page, int limit)
    {
        return await tripRepository.GetPaged(page, limit);
    }

    public async Task<int> GetCountTrips()
    {
        return await tripRepository.GetCount();
    }

    public async Task<(List<TripHistoryDto> Items, int TotalCount)> GetPagedHistoryByUserId(int userId, int page,
        int limit)
    {
        var clients = await clientRepository.GetClientByUserId(userId);
        var client = clients.FirstOrDefault();
        if (client == null) return ([], 0);

        return await tripRepository.GetHistoryByClientId(client.Id, page, limit);
    }

    public async Task<List<TripWithInfoDto>> GetTripWithInfo(int id)
    {
        return await tripRepository.GetTripWithDetailsById(id);
    }

    public async Task<CurrentTripDto?> GetActiveTripByClientId(int userId)
    {
        var client = await clientRepository.GetClientByUserId(userId);
        var clientId = client.Select(c => c.Id).FirstOrDefault();

        return await tripRepository.GetActiveTripDtoByClientId(clientId);
    }

    public async Task<TripFinishResult> FinishTripAsync(FinishTripRequest request)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var billId = await tripRepository.FinishTripAsync(
                request.TripId,
                request.Distance,
                request.EndLocation,
                request.FuelLevel
            );

            await unitOfWork.CommitTransactionAsync();

            var bill = await billRepository.GetById(billId);

            return new TripFinishResult(billId, bill?.Amount, "Поездка успешно завершена");
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> CancelTripAsync(int tripId)
    {
        await tripRepository.CancelTripAsync(tripId);
        return true;
    }

    public async Task<int> CreateTripAsync(TripCreateRequest request)
    {
        await unitOfWork.BeginTransactionAsync();
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

            var tripId = await tripRepository.Create(trip);

            var (tripDetail, errorTripDetail) = TripDetail.Create(
                0, tripId,
                request.StartLocation,
                request.EndLocation,
                request.InsuranceActive,
                request.FuelUsed,
                request.Refueled);

            if (!string.IsNullOrWhiteSpace(errorTripDetail)) throw new ArgumentException(errorTripDetail);

            await tripDetailRepository.Create(tripDetail);
            await carsService.MarkCarAsUnavailableAsync(request.CarId);

            await unitOfWork.CommitTransactionAsync();

            return tripId;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<int> UpdateTrip(int id, TripUpdateRequest request)
    {
        var tripId = await tripRepository.Update(
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

        var bookings = await bookingRepository.GetById(request.BookingId);
        var booking = bookings.FirstOrDefault()
                      ?? throw new NotFoundException("Booking not found for this trip");

        await carsService.MarkCarAsAvailableAsync(booking.CarId);

        return tripId;
    }

    public async Task<int> DeleteTrip(int id)
    {
        return await tripRepository.Delete(id);
    }
}