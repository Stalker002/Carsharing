using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface ITripService
{
    Task<List<Trip>> GetTrips();
    Task<int> CreateTrip(Trip trip);

    Task<int> UpdateTrip(int id, int? bookingId, int? statusId, string? tariffType, DateTime? startTime,
        DateTime? endTime, decimal? duration, decimal? distance);

    Task<int> DeleteTrip(int id);
}