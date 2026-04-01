using Carsharing.Application.DTOs;
using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface ICarRepository
{
    Task<List<Car>> Get(CancellationToken cancellationToken);

    Task<List<Car>> GetPaged(int page, int limit, CancellationToken cancellationToken);

    Task<List<Car>> GetById(int id, CancellationToken cancellationToken);

    Task<List<Car>> GetByCategoryId(List<int> categoryIds, CancellationToken cancellationToken);

    Task<List<Car>> GetPagedByCategoryId(List<int> categoryIds, int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCountByCategory(List<int> categoryIds, CancellationToken cancellationToken);

    Task<List<Car>> GetByStatusId(int statusId, CancellationToken cancellationToken);

    Task<List<CarWithInfoDto>> GetCarWithInfo(int id, CancellationToken cancellationToken);

    Task<List<CarWithInfoAdminDto>> GetCarWithInfoAdmin(int id, CancellationToken cancellationToken);

    Task<List<CarWithMinInfoDto>> GetPagedCarsByClients(int page, int limit, CancellationToken cancellationToken);

    Task<List<CarWithMinInfoDto>> GetCarWithMinInfoByCategoryIds(List<int> categoryIds, int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCount(CancellationToken cancellationToken);

    Task<int> Create(Car car, CancellationToken cancellationToken);

    Task<int> Update(int id, int? statusId, int? tariffId, int? categoryId, int? specificationId,
        string? location, decimal? fuelLevel, string? imagePath, CancellationToken cancellationToken);

    Task UpdateStatus(int? carId, int statusId, CancellationToken cancellationToken);

    Task<bool> TryUpdateStatus(int carId, int currentStatusId, int newStatusId, CancellationToken cancellationToken);

    Task<int> Delete(int id, CancellationToken cancellationToken);
}
