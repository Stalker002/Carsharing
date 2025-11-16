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

    public async Task<List<SpecificationCar>> GetSpecifications()
    {
        return await _specificationCarRepository.Get();
    }

    public async Task<int> CreateSpecification(SpecificationCar specificationCar)
    {
        return await _specificationCarRepository.Create(specificationCar);
    }

    public async Task<int> UpdateSpecification(int id, string? fuelType, string? brand, string? model,
        string? transmission, int? year, string? vinNumber, string? stateNumber, int? mileage, decimal? maxFuel,
        decimal? fuelPerKm)
    {
        return await _specificationCarRepository.Update(id, fuelType, brand, model, transmission, year, vinNumber,
            stateNumber, mileage, maxFuel, fuelPerKm);
    }

    public async Task<int> DeleteSpecification(int id)
    {
        return await _specificationCarRepository.Delete(id);
    }
}