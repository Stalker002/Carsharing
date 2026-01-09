using Carsharing.Application.Abstractions;
using Carsharing.Application.DTOs;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Enum;
using Carsharing.Core.Models;
using Carsharing.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.Application.Services;

public class CarsService : ICarsService
{
    private readonly ICarRepository _carRepository;
    private readonly ITariffRepository _tariffRepository;
    private readonly ISpecificationCarRepository _specificationCarRepository;
    private readonly ICarStatusRepository _statusRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IImageService _imageService;
    private readonly CarsharingDbContext _context;

    public CarsService(ICarRepository carRepository, ITariffRepository tariffRepository,
        ISpecificationCarRepository specificationCarRepository, ICarStatusRepository statusRepository,
        ICategoryRepository categoryRepository, CarsharingDbContext context, IImageService imageService)
    {
        _context = context;
        _imageService = imageService;
        _categoryRepository = categoryRepository;
        _statusRepository = statusRepository;
        _specificationCarRepository = specificationCarRepository;
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
        var response = await _context.Car
            .OrderBy(c => c.Id)
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(c => new CarWithMinInfoDto(
                c.Id,
                c.CarStatus!.Name,
                c.Tariff!.PricePerDay,
                c.Category!.Name,
                c.SpecificationCar!.FuelType!,
                c.SpecificationCar.MaxFuel!,
                c.SpecificationCar.Brand!,
                c.SpecificationCar.Model!,
                c.SpecificationCar.Transmission!,
                c.ImagePath
            ))
            .ToListAsync();

        return response;

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
        var car = await _carRepository.GetById(id);
        var tariffId = car.Select(ca => ca.TariffId).FirstOrDefault();
        var statusId = car.Select(ca => ca.CarStatusId).FirstOrDefault();
        var categoryId = car.Select(ca => ca.CategoryId).FirstOrDefault();
        var specificationId = car.Select(c => c.SpecificationId).FirstOrDefault();

        var tariff = await _tariffRepository.GetById(tariffId);

        var status = await _statusRepository.GetById(statusId);

        var category = await _categoryRepository.GetById(categoryId);

        var specification = await _specificationCarRepository.GetById(specificationId);

        var response = (from c in car
                        join t in tariff on c.TariffId equals t.Id
                        join st in status on c.CarStatusId equals st.Id
                        join sp in specification on c.SpecificationId equals sp.Id
                        join cat in category on c.CategoryId equals cat.Id
                        select new CarWithInfoDto(
                            c.Id,
                            st.Name,
                            t.PricePerMinute,
                            t.PricePerKm,
                            t.PricePerDay,
                            cat.Name,
                            sp.FuelType,
                            sp.Brand,
                            sp.Model,
                            sp.Transmission,
                            sp.Year,
                            sp.StateNumber,
                            sp.MaxFuel,
                            c.Location,
                            c.FuelLevel,
                            c.ImagePath)).ToList();

        return response;
    }

    public async Task<List<CarWithInfoAdminDto>> GetCarWithInfoAdmin(int id)
    {
        var car = await _carRepository.GetById(id);
        var tariffId = car.Select(ca => ca.TariffId).FirstOrDefault();
        var statusId = car.Select(ca => ca.CarStatusId).FirstOrDefault();
        var categoryId = car.Select(ca => ca.CategoryId).FirstOrDefault();
        var specificationId = car.Select(c => c.SpecificationId).FirstOrDefault();

        var tariff = await _tariffRepository.GetById(tariffId);

        var status = await _statusRepository.GetById(statusId);

        var category = await _categoryRepository.GetById(categoryId);

        var specification = await _specificationCarRepository.GetById(specificationId);

        var response = (from c in car
                        join t in tariff on c.TariffId equals t.Id
                        join st in status on c.CarStatusId equals st.Id
                        join sp in specification on c.SpecificationId equals sp.Id
                        join cat in category on c.CategoryId equals cat.Id
                        select new CarWithInfoAdminDto(
                            c.Id,
                            st.Id,
                            cat.Id,
                            sp.Transmission,
                            sp.Brand,
                            sp.Model,
                            sp.Year,
                            c.Location,
                            sp.VinNumber,
                            sp.StateNumber,
                            sp.FuelType,
                            c.FuelLevel,
                            sp.MaxFuel,
                            sp.FuelPerKm,
                            sp.Mileage,
                            t.Name,
                            t.PricePerMinute,
                            t.PricePerKm,
                            t.PricePerDay,
                            c.ImagePath
                            )).ToList();

        return response;
    }

    public async Task<List<CarWithMinInfoDto>> GetCarWithMinInfoByCategoryIds(List<int> categoryIds, int page, int limit)
    {
        var car = await _carRepository.GetPagedByCategoryId(categoryIds, page, limit);
        var tariffId = car.Select(ca => ca.TariffId).FirstOrDefault();
        var statusId = car.Select(ca => ca.CarStatusId).FirstOrDefault();
        var categoryId = car.Select(ca => ca.CategoryId).FirstOrDefault();
        var specificationId = car.Select(c => c.SpecificationId).FirstOrDefault();

        var tariff = await _tariffRepository.GetById(tariffId);

        var status = await _statusRepository.GetById(statusId);

        var category = await _categoryRepository.GetById(categoryId);

        var specification = await _specificationCarRepository.GetById(specificationId);

        var response = (from c in car
                        join t in tariff on c.TariffId equals t.Id
                        join st in status on c.CarStatusId equals st.Id
                        join sp in specification on c.SpecificationId equals sp.Id
                        join cat in category on c.CategoryId equals cat.Id
                        select new CarWithMinInfoDto(
                            c.Id,
                            st.Name,
                            t.PricePerDay,
                            cat.Name,
                            sp.FuelType,
                            sp.MaxFuel,
                            sp.Brand,
                            sp.Model,
                            sp.Transmission,
                            c.ImagePath)).ToList();

        return response;
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
        await using var transaction = await _context.Database.BeginTransactionAsync();
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

            await transaction.CommitAsync();
            return (carId, null)!;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

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
        await using var transaction = await _context.Database.BeginTransactionAsync();
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

            await transaction.CommitAsync();

            return (true, null)!;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

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