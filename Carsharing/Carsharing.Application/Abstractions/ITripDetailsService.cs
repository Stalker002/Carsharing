using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface ITripDetailsService
{
    Task<List<TripDetail>> GetTripDetails();
    Task<int> CreateTripDetail(TripDetail tripDetail);

    Task<int> UpdateTripDetail(int id, int? tripId, string? startLocation, string? endLocation,
        bool? insuranceActive, decimal? fuelUsed, decimal? refueled);

    Task<int> DeleteTripDetail(int id);
}