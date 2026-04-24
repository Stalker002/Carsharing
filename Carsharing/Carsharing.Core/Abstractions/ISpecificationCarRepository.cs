using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface ISpecificationCarRepository
{
    Task<List<SpecificationCar>> Get(CancellationToken cancellationToken);

    Task<List<SpecificationCar>> GetById(int id, CancellationToken cancellationToken);

    Task<int> Create(SpecificationCar specificationCar, CancellationToken cancellationToken);

    Task<int> Update(int id, string? fuelType, string? brand, string? model, string? transmission,
        int? year, string? vinNumber, string? stateNumber, int? mileage, decimal? maxFuel, decimal? fuelPerKm,
        CancellationToken cancellationToken);

    Task<int> Delete(int id, CancellationToken cancellationToken);
}