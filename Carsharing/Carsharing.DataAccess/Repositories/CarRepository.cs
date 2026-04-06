using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Cars;
using Shared.Enums;

namespace Carsharing.DataAccess.Repositories;

public class CarRepository(CarsharingDbContext context) : ICarRepository
{
    public async Task<List<Car>> Get(CancellationToken cancellationToken)
    {
        var carEntities = await context.Car
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
                c.FuelLevel,
                c.ImagePath).car)
            .ToList();

        return cars;
    }

    public async Task<List<Car>> GetPaged(int page, int limit, CancellationToken cancellationToken)
    {
        var carEntities = await context.Car
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
                c.FuelLevel,
                c.ImagePath).car)
            .ToList();

        return cars;
    }

    public async Task<int> GetCount(CancellationToken cancellationToken)
    {
        return await context.Car.CountAsync(cancellationToken);
    }

    public async Task<List<Car>> GetById(int id, CancellationToken cancellationToken)
    {
        var carEntities = await context.Car
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
                c.FuelLevel,
                c.ImagePath).car)
            .ToList();

        return cars;
    }

    public async Task<List<Car>> GetByCategoryId(List<int> categoryIds, CancellationToken cancellationToken)
    {
        var carEntities = await context.Car
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
                c.FuelLevel,
                c.ImagePath).car)
            .ToList();

        return cars;
    }

    public async Task<List<Car>> GetPagedByCategoryId(List<int> categoryIds, int page, int limit, CancellationToken cancellationToken)
    {
        var carEntities = await context.Car
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
                c.FuelLevel,
                c.ImagePath).car)
            .ToList();

        return cars;
    }

    public async Task<int> GetCountByCategory(List<int> categoryIds, CancellationToken cancellationToken)
    {
        return await context.Car
            .Where(c => categoryIds.Contains(c.CategoryId))
            .CountAsync(cancellationToken);
    }

    public async Task<List<CarWithInfoDto>> GetCarWithInfo(int id, CancellationToken cancellationToken)
    {
        return await context.Car
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
                c.FuelLevel,
                c.ImagePath
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<CarWithInfoAdminDto>> GetCarWithInfoAdmin(int id, CancellationToken cancellationToken)
    {
        return await context.Car
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
        return await context.Car
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
                c.ImagePath
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<CarWithMinInfoDto>> GetCarWithMinInfoByCategoryIds(List<int> categoryIds, int page, int limit, CancellationToken cancellationToken)
    {
        return await context.Car
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
                c.ImagePath
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Car>> GetByStatusId(int statusId, CancellationToken cancellationToken)
    {
        var carEntities = await context.Car
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
            ImagePath = car.ImagePath
        };

        await context.Car.AddAsync(carEntity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return carEntity.Id;
    }

    public async Task<int> Update(int id, int? statusId, int? tariffId, int? categoryId, int? specificationId,
        string? location, decimal? fuelLevel, string? imagePath, CancellationToken cancellationToken)
    {
        var car = await context.Car.FirstOrDefaultAsync(c => c.Id == id, cancellationToken: cancellationToken)
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
            car.FuelLevel,
            car.ImagePath);

        if (!string.IsNullOrWhiteSpace(error))
            throw new ArgumentException($"Create exception car: {error}");

        await context.SaveChangesAsync(cancellationToken);

        return car.Id;
    }

    public async Task UpdateStatus(int? carId, int statusId, CancellationToken cancellationToken)
    {
        var car = await context.Car.FindAsync([carId], cancellationToken: cancellationToken) ?? throw new Exception("Автомобиль не найден");
        car.StatusId = statusId;
        context.Car.Update(car);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> TryUpdateStatus(int carId, int currentStatusId, int newStatusId, CancellationToken cancellationToken)
    {
        if (!context.Database.IsRelational())
        {
            var car = await context.Car.FirstOrDefaultAsync(c => c.Id == carId && c.StatusId == currentStatusId, cancellationToken: cancellationToken);
            if (car == null)
                return false;

            car.StatusId = newStatusId;
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }

        var affectedRows = await context.Car
            .Where(c => c.Id == carId && c.StatusId == currentStatusId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(c => c.StatusId, newStatusId), cancellationToken: cancellationToken);

        return affectedRows > 0;
    }

    public async Task<int> Delete(int id, CancellationToken cancellationToken)
    {
        await context.Car
            .Where(t => t.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return id;
    }
}
