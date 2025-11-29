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

    Task<int> GetCount();
    Task<int> Create(Car car);
    Task<int> Update(int id, int? statusId, int? tariffId, int? categoryId, int? specificationId,
        string? location, decimal? fuelLevel);

    Task<int> Delete(int id);
}