using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface ISpecificationsCarService
{
    Task<List<SpecificationCar>> GetSpecifications();
    Task<int> CreateSpecification(SpecificationCar specificationCar);

    Task<int> UpdateSpecification(int id, string? fuelType, string? brand, string? model,
        string? transmission, int? year, string? vinNumber, string? stateNumber, int? mileage, decimal? maxFuel,
        decimal? fuelPerKm);

    Task<int> DeleteSpecification(int id);
}