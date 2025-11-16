using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface ISpecificationCarRepository
{
    Task<List<SpecificationCar>> Get();
    Task<int> Create(SpecificationCar specificationCar);

    Task<int> Update(int id, string? fuelType, string? brand, string? model, string? transmission,
        int? year, string? vinNumber, string? stateNumber, int? mileage, decimal? maxFuel, decimal? fuelPerKm);

    Task<int> Delete(int id);
}