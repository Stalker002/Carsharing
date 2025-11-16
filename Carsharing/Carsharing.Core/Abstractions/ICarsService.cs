using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface ICarsService
{
    Task<List<Car>> GetCars();
    Task<int> CreateCar(Car car);

    Task<int> UpdateCar(int id, int? statusId, int? tariffId, int? categoryId, int? specificationId,
        string? location, decimal? fuelLevel);

    Task<int> DeleteCar(int id);
}