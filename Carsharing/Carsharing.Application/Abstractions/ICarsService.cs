using Carsharing.Application.DTOs;
using Carsharing.Core.Models;
using Shared.Contracts.Cars;

namespace Carsharing.Application.Abstractions;

public interface ICarsService
{
    Task<List<Car>> GetCars(CancellationToken cancellationToken);

    Task<List<Car>> GetPagedCars(int page, int limit, CancellationToken cancellationToken);

    Task<List<CarWithMinInfoDto>> GetPagedCarsByClients(int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCount(CancellationToken cancellationToken);

    Task<List<Car>> GetCarById(int id, CancellationToken cancellationToken);

    Task<List<CarWithInfoDto>> GetCarWithInfo(int id, CancellationToken cancellationToken);

    Task<List<CarWithInfoAdminDto>> GetCarWithInfoAdmin(int id, CancellationToken cancellationToken);

    Task<List<CarWithMinInfoDto>> GetCarWithMinInfoByCategoryIds(List<int> categoryIds, int page, int limit,
        CancellationToken cancellationToken);

    Task<int> GetCountByCategory(List<int> categoryIds, CancellationToken cancellationToken);

    Task<int> CreateCar(Car car, CancellationToken cancellationToken);

    Task<(int? Id, string Error)> CreateCarFullAsync(CarsCreateRequest request, CancellationToken cancellationToken);

    Task<(bool IsSuccess, string Error)> UpdateCarFullAsync(int id, CarUpdateDto request,
        CancellationToken cancellationToken);

    Task MarkCarAsUnavailableAsync(int? carId, CancellationToken cancellationToken);

    Task MarkCarAsAvailableAsync(int? carId, CancellationToken cancellationToken);

    Task UpdateCarLocationAsync(int carId, string location, double latitude, double longitude,
        CancellationToken cancellationToken);

    Task<int> DeleteCar(int id, CancellationToken cancellationToken);
}
