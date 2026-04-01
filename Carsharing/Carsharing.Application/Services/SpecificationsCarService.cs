using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class SpecificationsCarService : ISpecificationsCarService
{
    private readonly ISpecificationCarRepository _specificationCarRepository;

    public SpecificationsCarService(ISpecificationCarRepository specificationCarRepository)
    {
        _specificationCarRepository = specificationCarRepository;
    }

    public async Task<List<SpecificationCar>> GetSpecifications(CancellationToken cancellationToken)
    {
        return await _specificationCarRepository.Get(cancellationToken);
    }

    public async Task<int> CreateSpecification(SpecificationCar specificationCar, CancellationToken cancellationToken)
    {
        return await _specificationCarRepository.Create(specificationCar, cancellationToken);
    }

    public async Task<int> UpdateSpecification(int id, string? fuelType, string? brand, string? model,
        string? transmission, int? year, string? vinNumber, string? stateNumber, int? mileage, decimal? maxFuel,
        decimal? fuelPerKm, CancellationToken cancellationToken)
    {
        return await _specificationCarRepository.Update(id, fuelType, brand, model, transmission, year, vinNumber,
            stateNumber, mileage, maxFuel, fuelPerKm, cancellationToken);
    }

    public async Task<int> DeleteSpecification(int id, CancellationToken cancellationToken)
    {
        return await _specificationCarRepository.Delete(id, cancellationToken);
    }
}
