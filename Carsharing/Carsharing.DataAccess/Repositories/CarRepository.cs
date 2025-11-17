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
                c.FuelLevel).car)
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
            car.FuelLevel);

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
            FuelLevel = car.FuelLevel
        };

        await _context.Car.AddAsync(carEntity);
        await _context.SaveChangesAsync();

        return carEntity.Id;
    }

    public async Task<int> Update(int id, int? statusId, int? tariffId, int? categoryId, int? specificationId,
        string? location, decimal? fuelLevel)
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