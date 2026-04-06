using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface ISpecificationsCarService
{
    Task<List<SpecificationCar>> GetSpecifications(CancellationToken cancellationToken);

    Task<int> CreateSpecification(SpecificationCar specificationCar, CancellationToken cancellationToken);

    Task<int> UpdateSpecification(int id, string? fuelType, string? brand, string? model,
        string? transmission, int? year, string? vinNumber, string? stateNumber, int? mileage, decimal? maxFuel,
        decimal? fuelPerKm, CancellationToken cancellationToken);

    Task<int> DeleteSpecification(int id, CancellationToken cancellationToken);
}