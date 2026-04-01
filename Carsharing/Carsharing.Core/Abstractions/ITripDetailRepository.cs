using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface ITripDetailRepository
{
    Task<List<TripDetail>> Get(CancellationToken cancellationToken);

    Task<int> GetCarIdByTripId(int tripId, CancellationToken cancellationToken);

    Task<int> Create(TripDetail tripDetail, CancellationToken cancellationToken);

    Task<int> Update(int id, int? tripId, string? startLocation, string? endLocation,
        bool? insuranceActive, decimal? fuelUsed, decimal? refueled, CancellationToken cancellationToken);

    Task<int> Delete(int id, CancellationToken cancellationToken);
}