using Carsharing.Application.Abstractions;
using Carsharing.Application.DTOs;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Enum;
using Carsharing.Core.Models;
using Carsharing.DataAccess;

namespace Carsharing.Application.Services;

public class CarsService : ICarsService
{
    private readonly ICarRepository _carRepository;
    private readonly ITariffRepository _tariffRepository;
    private readonly ISpecificationCarRepository _specificationCarRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;

    public CarsService(ICarRepository carRepository, ITariffRepository tariffRepository,
        ISpecificationCarRepository specificationCarRepository, IUnitOfWork unitOfWork, IImageService imageService)
    {
        _imageService = imageService;
        _specificationCarRepository = specificationCarRepository;
        _unitOfWork = unitOfWork;
        _tariffRepository = tariffRepository;
        _carRepository = carRepository;
    }

    public async Task<List<Car>> GetCars()
    {
        return await _carRepository.Get();
    }

    public async Task<List<Car>> GetPagedCars(int page, int limit)
    {
        return await _carRepository.GetPaged(page, limit);
    }

    public async Task<List<CarWithMinInfoDto>> GetPagedCarsByClients(int page, int limit)
    {
        return await _carRepository.GetPagedCarsByClients(page, limit);
    }

    public async Task<int> GetCount()
    {
        return await _carRepository.GetCount();
    }

    public async Task<List<Car>> GetCarById(int id)
    {
        return await _carRepository.GetById(id);
    }

    public async Task<List<CarWithInfoDto>> GetCarWithInfo(int id)
    {
        return await _carRepository.GetCarWithInfo(id);
    }

    public async Task<List<CarWithInfoAdminDto>> GetCarWithInfoAdmin(int id)
    {
        return await _carRepository.GetCarWithInfoAdmin(id);
    }

    public async Task<List<CarWithMinInfoDto>> GetCarWithMinInfoByCategoryIds(List<int> categoryIds, int page, int limit)
    {
        return await _carRepository.GetCarWithMinInfoByCategoryIds(categoryIds, page, limit);
    }

    public async Task<List<Car>> GetPagedCarsByCategoryIds(List<int> categoryIds, int page, int limit)
    {
        return await _carRepository.GetPagedByCategoryId(categoryIds, page, limit);
    }

    public async Task<int> GetCountByCategory(List<int> categoryIds)
    {
        return await _carRepository.GetCountByCategory(categoryIds);
    }

    public async Task<int> CreateCar(Car car)
    {
        return await _carRepository.Create(car);
    }

    public async Task<(int? Id, string Error)> CreateCarFullAsync(CarsCreateRequest request)
    {
        await _unitOfWork.BeginTransactionAsync();
        string? savedImagePath = null;

        try
        {
            var (tariff, errorTariff) = Tariff.Create(
                0,
                request.TariffName,
                request.PricePerMinute,
                request.PricePerKm,
                request.PricePerDay);
            if (!string.IsNullOrEmpty(errorTariff)) return (null, errorTariff);

            var tariffId = await _tariffRepository.Create(tariff);

            var (spec, errorSpec) = SpecificationCar.Create(
                0,
                request.FuelType,
                request.Brand,
                request.Model,
                request.Transmission,
                request.Year,
                request.VinNumber,
                request.StateNumber,
                request.Mileage,
                request.MaxFuel,
                request.FuelPerKm);
            if (!string.IsNullOrEmpty(errorSpec)) return (null, errorSpec);

            var specificationId = await _specificationCarRepository.Create(spec);

            if (request.Image is { Length: > 0 })
            {
                savedImagePath = await _imageService.SaveCarImageAsync(request.Image);
            }

            var (car, errorCar) = Car.Create(
                0,
                request.StatusId,
                tariffId,
                request.CategoryId,
                specificationId,
                request.Location,
                request.FuelLevel,
                savedImagePath);

            if (!string.IsNullOrEmpty(errorCar))
            {
                if (savedImagePath != null) _imageService.DeleteFile(savedImagePath);
                return (null, errorCar);
            }

            var carId = await _carRepository.Create(car);

            await _unitOfWork.CommitTransactionAsync();
            return (carId, null)!;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();

            if (savedImagePath != null) _imageService.DeleteFile(savedImagePath);

            if (ex.InnerException?.Message.Contains("23505") == true)
            {
                return (null, "Машина с таким номером или VIN уже существует.");
            }

            return (null, $"Internal error: {ex.Message}");
        }
    }

    public async Task<(bool IsSuccess, string Error)> UpdateCarFullAsync(int id, CarUpdateDto request)
    {
        await _unitOfWork.BeginTransactionAsync();
        string? newImagePathSystem = null;

        try
        {
            var carsCollection = await _carRepository.GetById(id);
            var car = carsCollection.FirstOrDefault();

            if (car == null) return (false, "Car not found");

            await _tariffRepository.Update(
                car.TariffId,
                request.TariffName,
                request.PricePerMinute,
                request.PricePerKm,
                request.PricePerDay
            );

            await _specificationCarRepository.Update(
                car.SpecificationId,
                request.FuelType,
                request.Brand,
                request.Model,
                request.Transmission,
                request.Year,
                request.VinNumber,
                request.StateNumber,
                (int)request.Mileage,
                request.MaxFuel,
                request.FuelPerKm
            );

            string? imagePathToUpdate = null;
            if (request.Image is { Length: > 0 })
            {
                imagePathToUpdate = await _imageService.SaveCarImageAsync(request.Image);
                newImagePathSystem = imagePathToUpdate;
            }

            await _carRepository.Update(
                id: id,
                statusId: request.StatusId,
                categoryId: request.CategoryId,
                tariffId: null,
                specificationId: null,
                location: request.Location,
                fuelLevel: request.FuelLevel,
                imagePath: imagePathToUpdate
            );

            await _unitOfWork.CommitTransactionAsync();

            return (true, null)!;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();

            if (newImagePathSystem != null)
            {
                _imageService.DeleteFile(newImagePathSystem);
            }

            if (ex.InnerException?.Message.Contains("23505") == true)
            {
                return (false, "Дубликат данных: VIN или Гос. номер уже занят.");
            }

            return ex is ArgumentException ? (false, ex.Message) : (false, $"Internal error: {ex.Message}");
        }
    }

    public async Task MarkCarAsUnavailableAsync(int? carId)
    {
        await _carRepository.UpdateStatus(carId, (int)CarStatusEnum.Reserved);
    }

    public async Task MarkCarAsAvailableAsync(int? carId)
    {
        await _carRepository.UpdateStatus(carId, (int)CarStatusEnum.Available);
    }

    public async Task<int> DeleteCar(int id)
    {
        return await _carRepository.Delete(id);
    }
}