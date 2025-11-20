using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface ICarRepository
{
    Task<List<Car>> Get();
    Task<int> Create(Car car);

    Task<List<Car>> GetById(int id);

    Task<int> GetCount();

    Task<int> Update(int id, int? statusId, int? tariffId, int? categoryId, int? specificationId,
        string? location, decimal? fuelLevel);

    Task<int> Delete(int id);
}