using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class TripService : ITripService
{
    private readonly ITripRepository _tripRepository;

    public TripService(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public async Task<List<Trip>> GetTrips()
    {
        return await _tripRepository.Get();
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