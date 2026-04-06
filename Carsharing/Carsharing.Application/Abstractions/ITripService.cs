using Carsharing.Core.Models;
using Shared.Contracts.Trip;

namespace Carsharing.Application.Abstractions;

public interface ITripService
{
    Task<List<Trip>> GetTrips(CancellationToken cancellationToken);

    Task<List<Trip>> GetPagedTrips(int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCountTrips(CancellationToken cancellationToken);

    Task<(List<TripHistoryDto> Items, int TotalCount)> GetPagedHistoryByUserId(int clientId, int page, int limit, CancellationToken cancellationToken);

    Task<List<TripWithInfoDto>> GetTripWithInfo(int id, CancellationToken cancellationToken);

    Task<CurrentTripDto?> GetActiveTripByClientId(int clientId, CancellationToken cancellationToken);

    Task<int> CreateTripAsync(int userId, TripCreateRequest request, CancellationToken cancellationToken);

    Task<TripFinishResult> FinishTripAsync(int userId, FinishTripRequest request, CancellationToken cancellationToken);

    Task<bool> CancelTripAsync(int tripId, CancellationToken cancellationToken);

    Task<int> UpdateTrip(int id, TripUpdateRequest request, CancellationToken cancellationToken);

    Task<int> DeleteTrip(int id, CancellationToken cancellationToken);
}
