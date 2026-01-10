using Carsharing.Application.DTOs;
using Carsharing.Contracts;
using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface ITripService
{
    Task<List<Trip>> GetTrips();
    Task<List<Trip>> GetPagedTrips(int page, int limit);
    Task<int> GetCountTrips();
    Task<(List<TripHistoryDto> Items, int TotalCount)> GetPagedHistoryByUserId(int clientId, int page, int limit);
    Task<List<TripWithInfoDto>> GetTripWithInfo(int id);
    Task<CurrentTripDto?> GetActiveTripByClientId(int clientId);
    Task<int> CreateTripAsync(TripCreateRequest request);
    Task<TripFinishResult> FinishTripAsync(FinishTripRequest request);
    Task<bool> CancelTripAsync(int tripId);
    Task<int> UpdateTrip(int id, TripUpdateRequest request);

    Task<int> DeleteTrip(int id);
}