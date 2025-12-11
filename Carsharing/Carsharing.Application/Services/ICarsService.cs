using Carsharing.Application.DTOs;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public interface ICarsService
{
    Task<List<Car>> GetCars();
    Task<List<Car>> GetPagedCars(int page, int limit);
    Task<List<CarWithMinInfoDto>> GetPagedCarsByClients(int page, int limit);
    Task<int> GetCount();
    Task<List<Car>> GetCarById(int id);
    Task<List<CarWithInfoDto>> GetCarWithInfo(int id);
    Task<List<CarWithInfoAdminDto>> GetCarWithInfoAdmin(int id);
    Task<List<CarWithMinInfoDto>> GetCarWithMinInfoByCategoryIds(List<int> categoryIds, int page, int limit);
    Task<List<Car>> GetPagedCarsByCategoryIds(List<int> categoryIds, int page, int limit);
    Task<int> GetCountByCategory(List<int> categoryIds);
    Task<int> CreateCar(Car car);

    Task<int> UpdateCar(int id, int? statusId, int? tariffId, int? categoryId, int? specificationId,
        string? location, decimal? fuelLevel, string? imagePath);

    Task<int> DeleteCar(int id);
}