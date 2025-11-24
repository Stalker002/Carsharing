using Carsharing.Application.DTOs;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public interface ICarsService
{
    Task<List<Car>> GetCars();
    Task<List<CarWithInfoDto>> GetCarWithInfo(int id);

    Task<List<Car>> GetCarsByCategoryIds(List<int> categoryIds);
    Task<int> GetCount();
    Task<int> CreateCar(Car car);

    Task<int> UpdateCar(int id, int? statusId, int? tariffId, int? categoryId, int? specificationId,
        string? location, decimal? fuelLevel);

    Task<int> DeleteCar(int id);
}