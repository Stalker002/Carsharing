using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class SpecificationCarRepository : ISpecificationCarRepository
{
    private readonly CarsharingDbContext _context;

    public SpecificationCarRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<SpecificationCar>> Get()
    {
        var specificationEntity = await _context.SpecificationCar
            .AsNoTracking()
            .ToListAsync();

        var specifications = specificationEntity
            .Select(sp => SpecificationCar.Create(
                sp.Id,
                sp.FuelType,
                sp.Brand,
                sp.Model,
                sp.Transmission,
                sp.Year,
                sp.VinNumber,
                sp.StateNumber,
                sp.Mileage,
                sp.MaxFuel,
                sp.FuelPerKm).specificationCar)
            .ToList();

        return specifications;
    }

    public async Task<int> Create(SpecificationCar specificationCar)
    {
        var (_, error) = SpecificationCar.Create(
            specificationCar.Id,
            specificationCar.FuelType,
            specificationCar.Brand,
            specificationCar.Model,
            specificationCar.Transmission,
            specificationCar.Year,
            specificationCar.VinNumber,
            specificationCar.StateNumber,
            specificationCar.Mileage,
            specificationCar.MaxFuel,
            specificationCar.FuelPerKm);

        if (!string.IsNullOrWhiteSpace(error))
            throw new ArgumentException($"Specification car create error: {error}");

        var specificationEntity = new SpecificationCarEntity
        {
            Id = specificationCar.Id,
            FuelType = specificationCar.FuelType,
            Brand = specificationCar.Brand,
            Model = specificationCar.Model,
            Transmission = specificationCar.Transmission,
            Year = specificationCar.Year,
            VinNumber = specificationCar.VinNumber,
            StateNumber = specificationCar.StateNumber,
            Mileage = specificationCar.Mileage,
            MaxFuel = specificationCar.MaxFuel,
            FuelPerKm = specificationCar.FuelPerKm
        };

        await _context.SpecificationCar.AddAsync(specificationEntity);
        await _context.SaveChangesAsync();

        return specificationEntity.Id;
    }

    public async Task<int> Update(int id, string? fuelType, string? brand, string? model, string? transmission,
        int? year, string? vinNumber, string? stateNumber, int? mileage, decimal? maxFuel, decimal? fuelPerKm)
    {
        var specification = await _context.SpecificationCar.FirstOrDefaultAsync(sp => sp.Id == id)
                            ?? throw new Exception("Specification not found");

        if (!string.IsNullOrWhiteSpace(fuelType))
            specification.FuelType = fuelType;

        if (!string.IsNullOrWhiteSpace(brand))
            specification.Brand = brand;

        if (!string.IsNullOrWhiteSpace(model))
            specification.Model = model;

        if (!string.IsNullOrWhiteSpace(transmission))
            specification.Transmission = transmission;

        if (year.HasValue)
            specification.Year = year.Value;

        if (!string.IsNullOrWhiteSpace(vinNumber))
            specification.VinNumber = vinNumber;

        if (!string.IsNullOrWhiteSpace(stateNumber))
            specification.StateNumber = stateNumber;

        if (mileage.HasValue)
            specification.Mileage = mileage.Value;

        if (maxFuel.HasValue)
            specification.MaxFuel = maxFuel.Value;

        if (fuelPerKm.HasValue)
            specification.FuelPerKm = fuelPerKm.Value;

        var (_, error) = SpecificationCar.Create(
            specification.Id,
            specification.FuelType,
            specification.Brand,
            specification.Model,
            specification.Transmission,
            specification.Year,
            specification.VinNumber,
            specification.StateNumber,
            specification.Mileage,
            specification.MaxFuel,
            specification.FuelPerKm);

        if (!string.IsNullOrWhiteSpace(error))
            throw new ArgumentException($"Specification car create error: {error}");

        await _context.SaveChangesAsync();

        return specification.Id;
    }

    public async Task<int> Delete(int id)
    {
        var specificationEntity = _context.SpecificationCar
            .Where(sp => sp.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}