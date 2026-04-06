using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface ITripDetailsService
{
    Task<List<TripDetail>> GetTripDetails(CancellationToken cancellationToken);

    Task<int> CreateTripDetail(TripDetail tripDetail, CancellationToken cancellationToken);

    Task<int> UpdateTripDetail(int id, int? tripId, string? startLocation, string? endLocation,
        bool? insuranceActive, decimal? fuelUsed, decimal? refueled, CancellationToken cancellationToken);

    Task<int> DeleteTripDetail(int id, CancellationToken cancellationToken);
}