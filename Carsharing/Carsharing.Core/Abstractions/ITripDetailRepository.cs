using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface ITripDetailRepository
{
    Task<List<TripDetail>> Get();
    Task<List<TripDetail>> GetByTripId(int tripId);
    Task<int> Create(TripDetail tripDetail);
    Task<int> Update(int id, int? tripId, string? startLocation, string? endLocation,
        bool? insuranceActive, decimal? fuelUsed, decimal? refueled);
    Task<int> Delete(int id);
}