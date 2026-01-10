using Carsharing.Application.DTOs;
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
    Task<int> Create(Trip trip);
    Task<int> Update(int id, int? bookingId, int? statusId, string? tariffType, DateTime? startTime,
        DateTime? endTime, decimal? duration, decimal? distance);
    Task<int> Delete(int id);
    Task<List<TripWithInfoDto>> GetTripWithDetailsById(int id);
    Task<(List<TripHistoryDto> Items, int TotalCount)> GetHistoryByClientId(int clientId, int page, int limit);
    Task<CurrentTripDto?> GetActiveTripDtoByClientId(int clientId);
    Task<int> FinishTripAsync(int tripId, decimal distance, string endLocation, decimal fuelLevel);
    Task CancelTripAsync(int tripId);
}