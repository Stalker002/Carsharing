using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class TripDetailsService : ITripDetailsService
{
    private readonly ITripDetailRepository _tripDetailRepository;

    public TripDetailsService(ITripDetailRepository tripDetailRepository)
    {
        _tripDetailRepository = tripDetailRepository;
    }

    public async Task<List<TripDetail>> GetTripDetails()
    {
        return await _tripDetailRepository.Get();
    }

    public async Task<int> CreateTripDetail(TripDetail tripDetail)
    {
        return await _tripDetailRepository.Create(tripDetail);
    }

    public async Task<int> UpdateTripDetail(int id, int? tripId, string? startLocation, string? endLocation,
        bool? insuranceActive, decimal? fuelUsed, decimal? refueled)
    {
        return await _tripDetailRepository.Update(id, tripId, startLocation, endLocation, insuranceActive, fuelUsed,
            refueled);
    }

    public async Task<int> DeleteTripDetail(int id)
    {
        return await _tripDetailRepository.Delete(id);
    }
}