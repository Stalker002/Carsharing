using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class CarRepository : ICarRepository
{
    private readonly CarsharingDbContext _context;

    public CarRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Car>> Get()
    {
        var carEntities = await _context.Car
            .AsNoTracking()
            .ToListAsync();

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

    public async Task<List<Car>> GetPaged(int page, int limit)
    {
        var carEntities = await _context.Car
            .AsNoTracking()
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

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

    public async Task<int> GetCount()
    {
        return await _context.Car.CountAsync();
    }

    public async Task<List<Car>> GetById(int id)
    {
        var carEntities = await _context.Car
            .Where(c => c.Id == id)
            .AsNoTracking()
            .ToListAsync();

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

    public async Task<List<Car>> GetByCategoryId(List<int> categoryIds)
    {
        var carEntities = await _context.Car
            .Where(c => categoryIds.Contains(c.CategoryId))
            .AsNoTracking()
            .ToListAsync();

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

    public async Task<List<Car>> GetPagedByCategoryId(List<int> categoryIds, int page, int limit)
    {
        var carEntities = await _context.Car
            .Where(c => categoryIds.Contains(c.CategoryId) && c.StatusId == 1)
            .AsNoTracking()
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

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

    public async Task<int> GetCountByCategory(List<int> categoryIds)
    {
        return await _context.Car
            .Where(c => categoryIds.Contains(c.CategoryId))
            .CountAsync();
    }

    public async Task<List<Car>> GetByStatusId(int statusId)
    {
        var carEntities = await _context.Car
            .Where(c => c.StatusId == statusId)
            .AsNoTracking()
            .ToListAsync();

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

    public async Task<int> Create(Car car)
    {
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

        var carEntity = new CarEntity
        {
            Id = car.Id,
            StatusId = car.StatusId,
            TariffId = car.TariffId,
            CategoryId = car.CategoryId,
            SpecificationId = car.SpecificationId,
            Location = car.Location,
            FuelLevel = car.FuelLevel,
            ImagePath = car.ImagePath
        };

        await _context.Car.AddAsync(carEntity);
        await _context.SaveChangesAsync();

        return carEntity.Id;
    }

    public async Task<int> Update(int id, int? statusId, int? tariffId, int? categoryId, int? specificationId,
        string? location, decimal? fuelLevel, string? imagePath)
    {
        var car = await _context.Car.FirstOrDefaultAsync(c => c.Id == id)
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

        await _context.SaveChangesAsync();

        return car.Id;
    }

    public async Task<int> Delete(int id)
    {
        var carEntity = await _context.Car
            .Where(t => t.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}