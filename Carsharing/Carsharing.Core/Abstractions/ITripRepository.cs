using Carsharing.Core.Models;
using Shared.Contracts.Trip;

namespace Carsharing.Core.Abstractions;

public interface ITripRepository
{
    Task<List<Trip>> Get(CancellationToken cancellationToken);

    Task<List<Trip>> GetPaged(int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCount(CancellationToken cancellationToken);

    Task<List<Trip>> GetById(int id, CancellationToken cancellationToken);

    Task<List<Trip>> GetByBookingId(List<int> bookingIds, CancellationToken cancellationToken);

    Task<int> GetCountByBooking(List<int> bookingIds, CancellationToken cancellationToken);

    Task<int> Create(Trip trip, CancellationToken cancellationToken);

    Task<int> Update(int id, int? bookingId, int? statusId, string? tariffType, DateTime? startTime,
        DateTime? endTime, decimal? duration, decimal? distance, CancellationToken cancellationToken);

    Task<int> Delete(int id, CancellationToken cancellationToken);

    Task<List<TripWithInfoDto>> GetTripWithDetailsById(int id, CancellationToken cancellationToken);

    Task<(List<TripHistoryDto> Items, int TotalCount)> GetHistoryByClientId(int clientId, int page, int limit,
        CancellationToken cancellationToken);

    Task<CurrentTripDto?> GetActiveTripDtoByClientId(int clientId, CancellationToken cancellationToken);

    Task<int> FinishTripAsync(int tripId, decimal distance, string endLocation, decimal fuelLevel,
        CancellationToken cancellationToken);

    Task CancelTripAsync(int tripId, CancellationToken cancellationToken);
}