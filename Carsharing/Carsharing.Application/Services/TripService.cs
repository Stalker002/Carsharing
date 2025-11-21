using Carsharing.Application.DTOs;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class TripService : ITripService
{
    private readonly ITripRepository _tripRepository;
    private readonly ITripDetailRepository _tripDetailRepository;

    public TripService(ITripRepository tripRepository, ITripDetailRepository tripDetailRepository)
    {
        _tripDetailRepository = tripDetailRepository;
        _tripRepository = tripRepository;
    }

    public async Task<List<Trip>> GetTrips()
    {
        return await _tripRepository.Get();
    }

    public async Task<List<TripWithInfoDto>> GetTripWithInfo(int id)
    {
        var trip = await _tripRepository.GetById(id);
        var tripDetail = await _tripDetailRepository.Get();

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