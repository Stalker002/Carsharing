using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Exceptions;
using Carsharing.Core.Models;
using Shared.Contracts.Trip;
using Shared.Enums;

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
    public async Task<List<Trip>> GetTrips(CancellationToken cancellationToken)
    {
        return await tripRepository.Get(cancellationToken);
    }

    public async Task<List<Trip>> GetPagedTrips(int page, int limit, CancellationToken cancellationToken)
    {
        return await tripRepository.GetPaged(page, limit, cancellationToken);
    }

    public async Task<int> GetCountTrips(CancellationToken cancellationToken)
    {
        return await tripRepository.GetCount(cancellationToken);
    }

    public async Task<(List<TripHistoryDto> Items, int TotalCount)> GetPagedHistoryByUserId(int userId, int page,
        int limit, CancellationToken cancellationToken)
    {
        var clients = await clientRepository.GetClientByUserId(userId, cancellationToken);
        var client = clients.FirstOrDefault();
        if (client == null) return ([], 0);

        return await tripRepository.GetHistoryByClientId(client.Id, page, limit, cancellationToken);
    }

    public async Task<List<TripWithInfoDto>> GetTripWithInfo(int id, CancellationToken cancellationToken)
    {
        return await tripRepository.GetTripWithDetailsById(id, cancellationToken);
    }

    public async Task<List<TripWithInfoDto>> GetTripWithInfo(int userId, int id, CancellationToken cancellationToken)
    {
        var client = (await clientRepository.GetClientByUserId(userId, cancellationToken)).FirstOrDefault()
                     ?? throw new NotFoundException("Client not found");

        var trip = (await tripRepository.GetById(id, cancellationToken)).FirstOrDefault()
                   ?? throw new NotFoundException("Trip not found");

        var booking = (await bookingRepository.GetById(trip.BookingId, cancellationToken)).FirstOrDefault()
                      ?? throw new NotFoundException("Booking not found");

        if (booking.ClientId != client.Id)
            throw new UnauthorizedAccessException("Trip does not belong to current user");

        return await tripRepository.GetTripWithDetailsById(id, cancellationToken);
    }

    public async Task<CurrentTripDto?> GetActiveTripByClientId(int userId, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetClientByUserId(userId, cancellationToken);
        var clientId = client.Select(c => c.Id).FirstOrDefault();

        return await tripRepository.GetActiveTripDtoByClientId(clientId, cancellationToken);
    }

    public async Task<TripFinishResult> FinishTripAsync(int userId, FinishTripRequest request,
        CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetClientByUserId(userId, cancellationToken);
        var clientId = client.Select(c => c.Id).FirstOrDefault();

        var trip = (await tripRepository.GetById(request.TripId, cancellationToken)).FirstOrDefault()
                   ?? throw new NotFoundException("Trip not found");
        var booking = (await bookingRepository.GetById(trip.BookingId, cancellationToken)).FirstOrDefault()
                      ?? throw new NotFoundException("Booking not found");

        if (booking.ClientId != clientId)
            throw new UnauthorizedAccessException("Trip does not belong to current user");

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var billId = await tripRepository.FinishTripAsync(
                request.TripId,
                request.Distance,
                request.EndLocation,
                request.FuelLevel,
                cancellationToken
            );

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            var bill = await billRepository.GetById(billId, cancellationToken);

            return new TripFinishResult(billId, bill?.Amount, "Поездка успешно завершена");
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<bool> CancelTripAsync(int tripId, CancellationToken cancellationToken)
    {
        await tripRepository.CancelTripAsync(tripId, cancellationToken);
        return true;
    }

    public async Task<int> CreateTripAsync(int userId, TripCreateRequest request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetClientByUserId(userId, cancellationToken);
        var clientId = client.Select(c => c.Id).FirstOrDefault();

        var booking = (await bookingRepository.GetById(request.BookingId, cancellationToken)).FirstOrDefault()
                      ?? throw new NotFoundException("Booking not found");

        if (booking.ClientId != clientId)
            throw new UnauthorizedAccessException("Booking does not belong to current user");

        await unitOfWork.BeginTransactionAsync(cancellationToken);
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

            var tripId = await tripRepository.Create(trip, cancellationToken);

            var (tripDetail, errorTripDetail) = TripDetail.Create(
                0, tripId,
                request.StartLocation,
                request.EndLocation,
                request.InsuranceActive,
                request.FuelUsed,
                request.Refueled);

            if (!string.IsNullOrWhiteSpace(errorTripDetail)) throw new ArgumentException(errorTripDetail);

            await tripDetailRepository.Create(tripDetail, cancellationToken);
            await carsService.MarkCarAsUnavailableAsync(booking.CarId, cancellationToken);

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return tripId;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<int> UpdateTrip(int id, TripUpdateRequest request, CancellationToken cancellationToken)
    {
        var tripId = await tripRepository.Update(
            id,
            request.BookingId,
            request.StatusId,
            request.TariffType,
            request.StartTime,
            request.EndTime,
            request.Duration,
            request.Distance, cancellationToken);

        var isFinished = request.StatusId == (int)TripStatusEnum.Finished;
        var isCancelled = request.StatusId == (int)TripStatusEnum.Cancelled;

        if (request.EndTime == null && !isFinished && !isCancelled)
            return tripId;

        var bookings = await bookingRepository.GetById(request.BookingId, cancellationToken);
        var booking = bookings.FirstOrDefault()
                      ?? throw new NotFoundException("Booking not found for this trip");

        await carsService.MarkCarAsAvailableAsync(booking.CarId, cancellationToken);

        return tripId;
    }

    public async Task<int> DeleteTrip(int id, CancellationToken cancellationToken)
    {
        return await tripRepository.Delete(id, cancellationToken);
    }
}