using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class CarsService : ICarsService
{
    private readonly ICarRepository _carRepository;

    public CarsService(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public async Task<List<Car>> GetCars()
    {
        return await _carRepository.Get();
    }

    public async Task<int> CreateCar(Car car)
    {
        return await _carRepository.Create(car);
    }

    public async Task<int> UpdateCar(int id, int? statusId, int? tariffId, int? categoryId, int? specificationId,
        string? location, decimal? fuelLevel)
    {
        return await _carRepository.Update(id, statusId, tariffId, categoryId, specificationId, location, fuelLevel);
    }

    public async Task<int> DeleteCar(int id)
    {
        return await _carRepository.Delete(id);
    }
}