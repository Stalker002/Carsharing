using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Exceptions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class TripDetailsService : ITripDetailsService
{
    private readonly IBillingLifecycleService _billingLifecycleService;
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly ITripDetailRepository _tripDetailRepository;

    public TripDetailsService(ITripDetailRepository tripDetailRepository, IInsuranceRepository insuranceRepository,
        IBillingLifecycleService billingLifecycleService)
    {
        _billingLifecycleService = billingLifecycleService;
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

        var normalizedTripDetail =
            await _billingLifecycleService.NormalizeTripDetailAsync(tripDetail, cancellationToken);

        if (!normalizedTripDetail.InsuranceActive)
        {
            var createdId = await _tripDetailRepository.Create(normalizedTripDetail, cancellationToken);
            await _billingLifecycleService.SyncCarFuelLevelAsync(normalizedTripDetail.TripId, cancellationToken);
            return createdId;
        }

        var insurances = await _insuranceRepository.GetActiveByCarId(carId, cancellationToken);

        if (insurances.Count == 0)
            throw new ConflictException("Нельзя начать поездку с опцией страховки: у автомобиля нет активного полиса");

        var resultId = await _tripDetailRepository.Create(normalizedTripDetail, cancellationToken);
        await _billingLifecycleService.SyncCarFuelLevelAsync(normalizedTripDetail.TripId, cancellationToken);

        return resultId;
    }

    public async Task<int> UpdateTripDetail(int id, int? tripId, string? startLocation, string? endLocation,
        bool? insuranceActive, decimal? fuelUsed, decimal? refueled, CancellationToken cancellationToken)
    {
        var existingTripDetail = (await _tripDetailRepository.Get(cancellationToken)).FirstOrDefault(detail => detail.Id == id)
                                 ?? throw new NotFoundException("Trip detail not found");
        var (tripDetail, error) = TripDetail.Create(
            existingTripDetail.Id,
            tripId ?? existingTripDetail.TripId,
            startLocation ?? existingTripDetail.StartLocation,
            endLocation ?? existingTripDetail.EndLocation,
            insuranceActive ?? existingTripDetail.InsuranceActive,
            fuelUsed ?? existingTripDetail.FuelUsed,
            refueled ?? existingTripDetail.Refueled);

        if (!string.IsNullOrWhiteSpace(error))
            throw new ArgumentException($"Trip detail create exception: {error}");

        var normalizedTripDetail =
            await _billingLifecycleService.NormalizeTripDetailAsync(tripDetail, cancellationToken);

        if (normalizedTripDetail.InsuranceActive)
        {
            var carId = await _tripDetailRepository.GetCarIdByTripId(normalizedTripDetail.TripId, cancellationToken);
            var insurances = await _insuranceRepository.GetActiveByCarId(carId, cancellationToken);

            if (insurances.Count == 0)
                throw new ConflictException(
                    "Нельзя начать поездку с опцией страховки: у автомобиля нет активного полиса");
        }

        var updatedId = await _tripDetailRepository.Update(
            id,
            normalizedTripDetail.TripId,
            normalizedTripDetail.StartLocation,
            normalizedTripDetail.EndLocation,
            normalizedTripDetail.InsuranceActive,
            normalizedTripDetail.FuelUsed,
            normalizedTripDetail.Refueled,
            cancellationToken);

        await _billingLifecycleService.SyncCarFuelLevelAsync(normalizedTripDetail.TripId, cancellationToken);

        return updatedId;
    }

    public async Task<int> DeleteTripDetail(int id, CancellationToken cancellationToken)
    {
        return await _tripDetailRepository.Delete(id, cancellationToken);
    }
}
