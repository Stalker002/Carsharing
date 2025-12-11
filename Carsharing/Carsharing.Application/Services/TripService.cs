using Carsharing.Application.DTOs;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class TripService : ITripService
{
    private readonly ITripRepository _tripRepository;
    private readonly ITripDetailRepository _tripDetailRepository;
    private readonly IStatusRepository _statusRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IClientRepository _clientRepository;

    public TripService(ITripRepository tripRepository, ITripDetailRepository tripDetailRepository,
        IStatusRepository statusRepository, IBookingRepository bookingRepository, IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
        _bookingRepository = bookingRepository;
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

    public async Task<List<TripWithMinInfoDto>> GetPagedTripWithMinInfoByUserId(int userId, int page, int limit)
    {
        var client = await _clientRepository.GetClientByUserId(userId);
        var clientId = client.Select(c => c.Id).FirstOrDefault();

        var bookings = await _bookingRepository.GetPagedByClientId(clientId, page, limit);
        var bookingId = bookings.Select(c => c.Id).ToList();

        var trips = await _tripRepository.GetByBookingId(bookingId);
        var tripId = trips.Select(tr => tr.Id).ToList();

        var tripDetail = await _tripDetailRepository.GetByTripId(tripId);

        var statuses = await _statusRepository.Get();

        var response = (from tr in trips
                        join d in tripDetail on tr.Id equals d.TripId
                        join s in statuses on tr.StatusId equals s.Id
                        select new TripWithMinInfoDto(
                            tr.Id,
                            tr.BookingId,
                            s.Name,
                            tr.TariffType,
                            tr.StartTime,
                            tr.EndTime,
                            tr.Duration,
                            tr.Distance
                        )).ToList();

        return response;
    }

    public async Task<int> GetCountPagedBillWithMinInfoByUser(int userId)
    {
        var client = await _clientRepository.GetClientByUserId(userId);
        var clientId = client.Select(c => c.Id).FirstOrDefault();

        var bookings = await _bookingRepository.GetByClientId(clientId);
        var bookingId = bookings.Select(c => c.Id).ToList();

        return await _tripRepository.GetCountByBooking(bookingId);
    }

    public async Task<List<TripWithInfoDto>> GetTripWithInfo(int id)
    {
        var trip = await _tripRepository.GetById(id);
        var tripId = trip.Select(tr => tr.Id).ToList();

        var tripDetail = await _tripDetailRepository.GetByTripId(tripId);

        var statuses = await _statusRepository.Get();

        var response = (from d in tripDetail
                        join tr in trip on d.TripId equals tr.Id
                        join s in statuses on tr.StatusId equals s.Id
                        select new TripWithInfoDto(
                tr.Id,
                tr.BookingId,
                s.Name,
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

    public async Task<int> CreateTrip(Trip trip)
    {
        return await _tripRepository.Create(trip);
    }

    public async Task<int> UpdateTrip(int id, int? bookingId, int? statusId, string? tariffType, DateTime? startTime,
        DateTime? endTime, decimal? duration, decimal? distance)
    {
        return await _tripRepository.Update(id, bookingId, statusId, tariffType, startTime, endTime, duration,
            distance);
    }

    public async Task<int> DeleteTrip(int id)
    {
        return await _tripRepository.Delete(id);
    }
}