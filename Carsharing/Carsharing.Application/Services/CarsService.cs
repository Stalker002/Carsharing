using Carsharing.Application.Abstractions;
using Carsharing.Application.DTOs;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Shared.Contracts.Cars;
using Shared.Enums;

namespace Carsharing.Application.Services;

public class CarsService : ICarsService
{
    private readonly ICarRepository _carRepository;
    private readonly IImageService _imageService;
    private readonly ISpecificationCarRepository _specificationCarRepository;
    private readonly ITariffRepository _tariffRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CarsService(ICarRepository carRepository, ITariffRepository tariffRepository,
        ISpecificationCarRepository specificationCarRepository, IUnitOfWork unitOfWork, IImageService imageService)
    {
        _imageService = imageService;
        _specificationCarRepository = specificationCarRepository;
        _unitOfWork = unitOfWork;
        _tariffRepository = tariffRepository;
        _carRepository = carRepository;
    }

    public async Task<List<Car>> GetCars(CancellationToken cancellationToken)
    {
        return await _carRepository.Get(cancellationToken);
    }

    public async Task<List<Car>> GetPagedCars(int page, int limit, CancellationToken cancellationToken)
    {
        return await _carRepository.GetPaged(page, limit, cancellationToken);
    }

    public async Task<List<CarWithMinInfoDto>> GetPagedCarsByClients(int page, int limit,
        CancellationToken cancellationToken)
    {
        return await _carRepository.GetPagedCarsByClients(page, limit, cancellationToken);
    }

    public async Task<int> GetCount(CancellationToken cancellationToken)
    {
        return await _carRepository.GetCount(cancellationToken);
    }

    public async Task<List<Car>> GetCarById(int id, CancellationToken cancellationToken)
    {
        return await _carRepository.GetById(id, cancellationToken);
    }

    public async Task<List<CarWithInfoDto>> GetCarWithInfo(int id, CancellationToken cancellationToken)
    {
        return await _carRepository.GetCarWithInfo(id, cancellationToken);
    }

    public async Task<List<CarWithInfoAdminDto>> GetCarWithInfoAdmin(int id, CancellationToken cancellationToken)
    {
        return await _carRepository.GetCarWithInfoAdmin(id, cancellationToken);
    }

    public async Task<List<CarWithMinInfoDto>> GetCarWithMinInfoByCategoryIds(List<int> categoryIds, int page,
        int limit, CancellationToken cancellationToken)
    {
        return await _carRepository.GetCarWithMinInfoByCategoryIds(categoryIds, page, limit, cancellationToken);
    }

    public async Task<int> GetCountByCategory(List<int> categoryIds, CancellationToken cancellationToken)
    {
        return await _carRepository.GetCountByCategory(categoryIds, cancellationToken);
    }

    public async Task<int> CreateCar(Car car, CancellationToken cancellationToken)
    {
        return await _carRepository.Create(car, cancellationToken);
    }

    public async Task<(int? Id, string Error)> CreateCarFullAsync(CarsCreateRequest request,
        CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
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

            var tariffId = await _tariffRepository.Create(tariff, cancellationToken);

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

            var specificationId = await _specificationCarRepository.Create(spec, cancellationToken);

            if (request.Image is { Length: > 0 })
                savedImagePath = await _imageService.SaveCarImageAsync(request.Image, cancellationToken);

            var (car, errorCar) = Car.Create(
                0,
                request.StatusId,
                tariffId,
                request.CategoryId,
                specificationId,
                request.Location,
                request.Latitude,
                request.Longitude,
                request.FuelLevel,
                savedImagePath);

            if (!string.IsNullOrEmpty(errorCar))
            {
                if (savedImagePath != null) await _imageService.DeleteFile(savedImagePath, cancellationToken);
                return (null, errorCar);
            }

            var carId = await _carRepository.Create(car, cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return (carId, string.Empty);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);

            if (savedImagePath != null) await _imageService.DeleteFile(savedImagePath, cancellationToken);

            if (ex.InnerException?.Message.Contains("23505") == true)
                return (null, "Машина с таким номером или VIN уже существует.");

            return (null, $"Internal error: {ex.Message}");
        }
    }

    public async Task<(bool IsSuccess, string Error)> UpdateCarFullAsync(int id, CarUpdateDto request,
        CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        string? newImagePathSystem = null;

        try
        {
            var carsCollection = await _carRepository.GetById(id, cancellationToken);
            var car = carsCollection.FirstOrDefault();

            if (car == null) return (false, "Car not found");

            await _tariffRepository.Update(
                car.TariffId,
                request.TariffName,
                request.PricePerMinute,
                request.PricePerKm,
                request.PricePerDay,
                cancellationToken
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
                request.Mileage,
                request.MaxFuel,
                request.FuelPerKm,
                cancellationToken
            );

            string? imagePathToUpdate = null;
            if (request.Image is { Length: > 0 })
            {
                imagePathToUpdate = await _imageService.SaveCarImageAsync(request.Image, cancellationToken);
                newImagePathSystem = imagePathToUpdate;
            }

            await _carRepository.Update(
                id,
                request.StatusId,
                categoryId: request.CategoryId,
                tariffId: null,
                specificationId: null,
                location: request.Location,
                latitude: request.Latitude,
                longitude: request.Longitude,
                fuelLevel: request.FuelLevel,
                imagePath: imagePathToUpdate,
                cancellationToken: cancellationToken
            );

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);

            if (newImagePathSystem != null) await _imageService.DeleteFile(newImagePathSystem, cancellationToken);

            if (ex.InnerException?.Message.Contains("23505") == true)
                return (false, "Дубликат данных: VIN или Гос. номер уже занят.");

            return ex is ArgumentException ? (false, ex.Message) : (false, $"Internal error: {ex.Message}");
        }
    }

    public async Task MarkCarAsUnavailableAsync(int? carId, CancellationToken cancellationToken)
    {
        await _carRepository.UpdateStatus(carId, (int)CarStatusEnum.Reserved, cancellationToken);
    }

    public async Task MarkCarAsAvailableAsync(int? carId, CancellationToken cancellationToken)
    {
        await _carRepository.UpdateStatus(carId, (int)CarStatusEnum.Available, cancellationToken);
    }

    public async Task<int> DeleteCar(int id, CancellationToken cancellationToken)
    {
        return await _carRepository.Delete(id, cancellationToken);
    }

    public async Task<List<Car>> GetPagedCarsByCategoryIds(List<int> categoryIds, int page, int limit,
        CancellationToken cancellationToken)
    {
        return await _carRepository.GetPagedByCategoryId(categoryIds, page, limit, cancellationToken);
    }
}