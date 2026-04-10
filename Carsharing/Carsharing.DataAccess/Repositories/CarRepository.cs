using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Shared.Contracts.Cars;
using Shared.Enums;

namespace Carsharing.DataAccess.Repositories;

public class CarRepository(CarsharingDbContext context) : ICarRepository
{
    private readonly CarsharingDbContext _context = context;


    private static double? GetLatitude(Point? point) => point?.Y;

    private static double? GetLongitude(Point? point) => point?.X;

    public async Task<List<Car>> Get(CancellationToken cancellationToken)
    {
        var carEntities = await _context.Car
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var cars = carEntities
            .Select(c => Car.Create(
                c.Id,
                c.StatusId,
                c.TariffId,
                c.CategoryId,
                c.SpecificationId,
                c.Location,
                GetLatitude(c.Coordinates),
                GetLongitude(c.Coordinates),
                c.FuelLevel,
                c.ImagePath).car)
            .ToList();

        return cars;
    }

    public async Task<List<Car>> GetPaged(int page, int limit, CancellationToken cancellationToken)
    {
        var carEntities = await _context.Car
            .AsNoTracking()
            .Skip((page - 1) * limit)
            .Take(limit)
            .OrderBy(c => c.Id)
            .ToListAsync(cancellationToken);

        var cars = carEntities
            .Select(c => Car.Create(
                c.Id,
                c.StatusId,
                c.TariffId,
                c.CategoryId,
                c.SpecificationId,
                c.Location,
                GetLatitude(c.Coordinates),
                GetLongitude(c.Coordinates),
                c.FuelLevel,
                c.ImagePath).car)
            .ToList();

        return cars;
    }

    public async Task<int> GetCount(CancellationToken cancellationToken)
    {
        return await _context.Car.CountAsync(cancellationToken);
    }

    public async Task<List<Car>> GetById(int id, CancellationToken cancellationToken)
    {
        var carEntities = await _context.Car
            .Where(c => c.Id == id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var cars = carEntities
            .Select(c => Car.Create(
                c.Id,
                c.StatusId,
                c.TariffId,
                c.CategoryId,
                c.SpecificationId,
                c.Location,
                GetLatitude(c.Coordinates),
                GetLongitude(c.Coordinates),
                c.FuelLevel,
                c.ImagePath).car)
            .ToList();

        return cars;
    }

    public async Task<List<Car>> GetByCategoryId(List<int> categoryIds, CancellationToken cancellationToken)
    {
        var carEntities = await _context.Car
            .Where(c => categoryIds.Contains(c.CategoryId))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var cars = carEntities
            .Select(c => Car.Create(
                c.Id,
                c.StatusId,
                c.TariffId,
                c.CategoryId,
                c.SpecificationId,
                c.Location,
                GetLatitude(c.Coordinates),
                GetLongitude(c.Coordinates),
                c.FuelLevel,
                c.ImagePath).car)
            .ToList();

        return cars;
    }

    public async Task<List<Car>> GetPagedByCategoryId(List<int> categoryIds, int page, int limit, CancellationToken cancellationToken)
    {
        var carEntities = await _context.Car
            .Where(c => categoryIds.Contains(c.CategoryId) && c.StatusId == (int)CarStatusEnum.Available)
            .AsNoTracking()
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync(cancellationToken);

        var cars = carEntities
            .Select(c => Car.Create(
                c.Id,
                c.StatusId,
                c.TariffId,
                c.CategoryId,
                c.SpecificationId,
                c.Location,
                GetLatitude(c.Coordinates),
                GetLongitude(c.Coordinates),
                c.FuelLevel,
                c.ImagePath).car)
            .ToList();

        return cars;
    }

    public async Task<int> GetCountByCategory(List<int> categoryIds, CancellationToken cancellationToken)
    {
        return await _context.Car
            .Where(c => categoryIds.Contains(c.CategoryId))
            .CountAsync(cancellationToken);
    }

    public async Task<List<CarWithInfoDto>> GetCarWithInfo(int id, CancellationToken cancellationToken)
    {
        return await _context.Car
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CarWithInfoDto(
                c.Id,
                c.CarStatus!.Name!,
                c.Tariff!.PricePerMinute,
                c.Tariff.PricePerKm,
                c.Tariff.PricePerDay,
                c.Category!.Name!,
                c.SpecificationCar!.FuelType!,
                c.SpecificationCar.Brand!,
                c.SpecificationCar.Model!,
                c.SpecificationCar.Transmission!,
                c.SpecificationCar.Year,
                c.SpecificationCar.StateNumber!,
                c.SpecificationCar.MaxFuel,
                c.Location,
                GetLatitude(c.Coordinates),
                GetLongitude(c.Coordinates),
                c.FuelLevel,
                c.ImagePath
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<CarWithInfoAdminDto>> GetCarWithInfoAdmin(int id, CancellationToken cancellationToken)
    {
        return await _context.Car
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CarWithInfoAdminDto(
                c.Id,
                c.StatusId,
                c.CategoryId,
                c.SpecificationCar!.Transmission,
                c.SpecificationCar.Brand,
                c.SpecificationCar.Model,
                c.SpecificationCar.Year,
                c.Location,
                GetLatitude(c.Coordinates),
                GetLongitude(c.Coordinates),
                c.SpecificationCar.VinNumber,
                c.SpecificationCar.StateNumber!,
                c.SpecificationCar.FuelType,
                c.FuelLevel,
                c.SpecificationCar.MaxFuel,
                c.SpecificationCar.FuelPerKm,
                c.SpecificationCar.Mileage,
                c.Tariff!.Name,
                c.Tariff.PricePerMinute,
                c.Tariff.PricePerKm,
                c.Tariff.PricePerDay,
                c.ImagePath
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<CarWithMinInfoDto>> GetPagedCarsByClients(int page, int limit, CancellationToken cancellationToken)
    {
        return await _context.Car
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(c => new CarWithMinInfoDto(
                c.Id,
                c.CarStatus!.Name!,
                c.Tariff!.PricePerDay,
                c.Category!.Name!,
                c.SpecificationCar!.FuelType!,
                c.SpecificationCar.MaxFuel,
                c.SpecificationCar.Brand!,
                c.SpecificationCar.Model!,
                c.SpecificationCar.Transmission!,
                GetLatitude(c.Coordinates),
                GetLongitude(c.Coordinates),
                c.ImagePath
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<CarWithMinInfoDto>> GetCarWithMinInfoByCategoryIds(List<int> categoryIds, int page, int limit, CancellationToken cancellationToken)
    {
        return await _context.Car
            .AsNoTracking()
            .Where(c => categoryIds.Contains(c.CategoryId))
            .Where(c => c.StatusId == (int)CarStatusEnum.Available)
            .OrderBy(c => c.Id)
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(c => new CarWithMinInfoDto(
                c.Id,
                c.CarStatus!.Name!,
                c.Tariff!.PricePerDay,
                c.Category!.Name!,
                c.SpecificationCar!.FuelType!,
                c.SpecificationCar.MaxFuel,
                c.SpecificationCar.Brand!,
                c.SpecificationCar.Model!,
                c.SpecificationCar.Transmission!,
                GetLatitude(c.Coordinates),
                GetLongitude(c.Coordinates),
                c.ImagePath
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Car>> GetByStatusId(int statusId, CancellationToken cancellationToken)
    {
        var carEntities = await _context.Car
            .Where(c => c.StatusId == statusId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var cars = carEntities
            .Select(c => Car.Create(
                c.Id,
                c.StatusId,
                c.TariffId,
                c.CategoryId,
                c.SpecificationId,
                c.Location,
                GetLatitude(c.Coordinates),
                GetLongitude(c.Coordinates),
                c.FuelLevel,
                c.ImagePath).car)
            .ToList();

        return cars;
    }

    public async Task<int> Create(Car car, CancellationToken cancellationToken)
    {
        var (_, error) = Car.Create(
            car.Id,
            car.CarStatusId,
            car.TariffId,
            car.CategoryId,
            car.SpecificationId,
            car.Location,
            car.Latitude,
            car.Longitude,
            car.FuelLevel,
            car.ImagePath);

        if (!string.IsNullOrWhiteSpace(error))
            throw new ArgumentException($"Create exception car: {error}");

        var carEntity = new CarEntity
        {
            Id = car.Id,
            StatusId = car.CarStatusId,
            TariffId = car.TariffId,
            CategoryId = car.CategoryId,
            SpecificationId = car.SpecificationId,
            Location = car.Location,
            FuelLevel = car.FuelLevel,
            Coordinates = car.Latitude.HasValue && car.Longitude.HasValue
                ? new Point(car.Longitude.Value, car.Latitude.Value) { SRID = 4326 }
                : null,
            ImagePath = car.ImagePath
        };

        await _context.Car.AddAsync(carEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return carEntity.Id;
    }

    public async Task<int> Update(int id, int? statusId, int? tariffId, int? categoryId, int? specificationId,
        string? location, double? latitude, double? longitude, decimal? fuelLevel, string? imagePath, CancellationToken cancellationToken)
    {
        var car = await _context.Car.FirstOrDefaultAsync(c => c.Id == id, cancellationToken: cancellationToken)
                  ?? throw new Exception("Car not found");

        if (statusId.HasValue)
            car.StatusId = statusId.Value;

        if (tariffId.HasValue)
            car.TariffId = tariffId.Value;

        if (categoryId.HasValue)
            car.CategoryId = categoryId.Value;

        if (specificationId.HasValue)
            car.SpecificationId = specificationId.Value;

        if (!string.IsNullOrWhiteSpace(location))
            car.Location = location;

        if (latitude.HasValue != longitude.HasValue)
            throw new ArgumentException("Latitude and longitude must be provided together");

        if (latitude.HasValue && longitude.HasValue)
            car.Coordinates = new Point(longitude.Value, latitude.Value) { SRID = 4326 };

        if (fuelLevel.HasValue)
            car.FuelLevel = fuelLevel.Value;

        if (!string.IsNullOrWhiteSpace(imagePath))
            car.ImagePath = imagePath;

        var (_, error) = Car.Create(
            car.Id,
            car.StatusId,
            car.TariffId,
            car.CategoryId,
            car.SpecificationId,
            car.Location,
            GetLatitude(car.Coordinates),
            GetLongitude(car.Coordinates),
            car.FuelLevel,
            car.ImagePath);

        if (!string.IsNullOrWhiteSpace(error))
            throw new ArgumentException($"Create exception car: {error}");

        await _context.SaveChangesAsync(cancellationToken);

        return car.Id;
    }

    public async Task UpdateStatus(int? carId, int statusId, CancellationToken cancellationToken)
    {
        var car = await _context.Car.FindAsync([carId], cancellationToken: cancellationToken);
        if (car == null)
            throw new Exception("Автомобиль не найден");

        car.StatusId = statusId;
        _context.Car.Update(car);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> TryUpdateStatus(int carId, int currentStatusId, int newStatusId, CancellationToken cancellationToken)
    {
        if (!context.Database.IsRelational())
        {
            var car = await _context.Car.FirstOrDefaultAsync(c => c.Id == carId && c.StatusId == currentStatusId, cancellationToken: cancellationToken);
            if (car == null)
                return false;

            car.StatusId = newStatusId;
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        var affectedRows = await _context.Car
            .Where(c => c.Id == carId && c.StatusId == currentStatusId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(c => c.StatusId, newStatusId), cancellationToken: cancellationToken);

        return affectedRows > 0;
    }

    public async Task<int> Delete(int id, CancellationToken cancellationToken)
    {
        await _context.Car
            .Where(t => t.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return id;
    }
}
