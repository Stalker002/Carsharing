using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface ITripRepository
{
    Task<List<Trip>> Get();
    Task<List<Trip>> GetPaged(int page, int limit);
    Task<int> GetCount();
    Task<List<Trip>> GetById(int id);
    Task<List<Trip>> GetByBookingId(List<int> bookingIds);
    Task<int> GetCountByBooking(List<int> bookingIds);
    Task<List<TripWithInfoDto>> GetTripWithDetailsById(int id);
    Task<TripEntity?> GetByIdWithDetails(int id);
    Task<CurrentTripDto?> GetActiveTripDtoByClientId(int clientId);
    Task<(List<TripHistoryDto> Items, int TotalCount)> GetHistoryByClientId(int clientId, int page, int limit);
    Task<int> Create(Trip trip);

    Task<int> Update(int id, int? bookingId, int? statusId, string? tariffType, DateTime? startTime,
        DateTime? endTime, decimal? duration, decimal? distance);

    Task<int> Delete(int id);
}