using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Exceptions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class TripDetailsService : ITripDetailsService
{
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly ITripDetailRepository _tripDetailRepository;

    public TripDetailsService(ITripDetailRepository tripDetailRepository, IInsuranceRepository insuranceRepository)
    {
        _insuranceRepository = insuranceRepository;
        _tripDetailRepository = tripDetailRepository;
    }

    public async Task<List<TripDetail>> GetTripDetails(CancellationToken cancellationToken)
    {
        return await _tripDetailRepository.Get(cancellationToken);
    }

    public async Task<int> CreateTripDetail(TripDetail tripDetail, CancellationToken cancellationToken)
    {
        var carId = await _tripDetailRepository.GetCarIdByTripId(tripDetail.TripId, cancellationToken);

        if (carId == 0)
            throw new NotFoundException($"Поездка с ID {tripDetail.TripId} или связанный автомобиль не найдены");

        if (!tripDetail.InsuranceActive) return await _tripDetailRepository.Create(tripDetail, cancellationToken);

        var insurances = await _insuranceRepository.GetActiveByCarId(carId, cancellationToken);

        if (insurances.Count == 0)
            throw new ConflictException("Нельзя начать поездку с опцией страховки: у автомобиля нет активного полиса");

        return await _tripDetailRepository.Create(tripDetail, cancellationToken);
    }

    public async Task<int> UpdateTripDetail(int id, int? tripId, string? startLocation, string? endLocation,
        bool? insuranceActive, decimal? fuelUsed, decimal? refueled, CancellationToken cancellationToken)
    {
        return await _tripDetailRepository.Update(id, tripId, startLocation, endLocation, insuranceActive, fuelUsed,
            refueled, cancellationToken);
    }

    public async Task<int> DeleteTripDetail(int id, CancellationToken cancellationToken)
    {
        return await _tripDetailRepository.Delete(id, cancellationToken);
    }
}