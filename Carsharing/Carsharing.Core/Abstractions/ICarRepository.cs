using Carsharing.Application.DTOs;
using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface ICarRepository
{
    Task<List<Car>> Get();
    Task<List<Car>> GetPaged(int page, int limit);
    Task<List<Car>> GetById(int id);
    Task<List<Car>> GetByCategoryId(List<int> categoryIds);
    Task<List<Car>> GetPagedByCategoryId(List<int> categoryIds, int page, int limit);
    Task<int> GetCountByCategory(List<int> categoryIds);
    Task<List<Car>> GetByStatusId(int statusId);
    Task<List<CarWithInfoDto>> GetCarWithInfo(int id);
    Task<List<CarWithInfoAdminDto>> GetCarWithInfoAdmin(int id);
    Task<List<CarWithMinInfoDto>> GetPagedCarsByClients(int page, int limit);
    Task<List<CarWithMinInfoDto>> GetCarWithMinInfoByCategoryIds(List<int> categoryIds, int page, int limit);
    Task<int> GetCount();
    Task<int> Create(Car car);
    Task<int> Update(int id, int? statusId, int? tariffId, int? categoryId, int? specificationId,
        string? location, decimal? fuelLevel, string? imagePath);

    Task UpdateStatus(int? carId, int statusId);
    Task<int> Delete(int id);
}