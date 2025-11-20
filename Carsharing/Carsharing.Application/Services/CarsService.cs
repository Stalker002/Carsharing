using Carsharing.Application.DTOs;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class CarsService : ICarsService
{
    private readonly ICarRepository _carRepository;
    private readonly ITariffRepository _tariffRepository;
    private readonly ISpecificationCarRepository _specificationCarRepository;
    private readonly IStatusRepository _statusRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CarsService(ICarRepository carRepository, ITariffRepository tariffRepository,
        ISpecificationCarRepository specificationCarRepository, IStatusRepository statusRepository,
        ICategoryRepository categoryRepository)
    {
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

    public async Task<List<CarWithInfoDto>> GetCarWithInfo(int id)
    {
        var car = await _carRepository.GetById(id);
        var tariff = await _tariffRepository.Get();
        var status = await _statusRepository.Get();
        var category = await _categoryRepository.Get();
        var specification = await _specificationCarRepository.Get();

        var response = (from c in car
            join t in tariff on c.TariffId equals t.Id
            join st in status on c.StatusId equals st.Id
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
                $"{sp.Brand} {sp.Model}",
                sp.Transmission,
                sp.Year,
                sp.VinNumber,
                sp.StateNumber,
                sp.Mileage,
                sp.MaxFuel,
                sp.FuelPerKm,
                c.Location,
                c.FuelLevel)).ToList();

        return response;
    }
    //$"{c.Brand} {c.Model} ({c.StateNumber})",

    public async Task<int> GetCount()
    {
        return await _carRepository.GetCount();
    }

    public async Task<int> CreateCar(Car car)
    {
        return await _carRepository.Create(car);
    }

    public async Task<int> UpdateCar(int id, int? statusId, int? tariffId, int? categoryId, int? specificationId,
        string? location, decimal? fuelLevel)
    {
        return await _carRepository.Update(id, statusId, tariffId, categoryId, specificationId, location, fuelLevel);
    }

    public async Task<int> DeleteCar(int id)
    {
        return await _carRepository.Delete(id);
    }
}