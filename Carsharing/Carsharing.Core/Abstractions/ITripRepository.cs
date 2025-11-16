using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface ITripRepository
{
    Task<List<Trip>> Get();
    Task<int> Create(Trip trip);

    Task<int> Update(int id, int? bookingId, int? statusId, string? tariffType, DateTime? startTime,
        DateTime? endTime, decimal? duration, decimal? distance);

    Task<int> Delete(int id);
}