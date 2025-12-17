using Carsharing.Application.DTOs;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public interface ITripService
{
    Task<List<Trip>> GetTrips();
    Task<List<Trip>> GetPagedTrips(int page, int limit);
    Task<int> GetCountTrips();
    Task<List<TripWithMinInfoDto>> GetPagedTripWithMinInfoByUserId(int userId, int page, int limit);
    Task<int> GetCountPagedBillWithMinInfoByUser(int userId);
    Task<List<TripWithInfoDto>> GetTripWithInfo(int id);
    Task<CurrentTripDto?> GetActiveTripByClientId(int clientId);
    Task<int> CreateTrip(Trip trip);
    Task<TripFinishResult> FinishTripAsync(FinishTripRequest request);
    Task<int> UpdateTrip(int id, int? bookingId, int? statusId, string? tariffType, DateTime? startTime,
        DateTime? endTime, decimal? duration, decimal? distance);

    Task<int> DeleteTrip(int id);
}