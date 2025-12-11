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

    public async Task<List<Car>> GetPagedCars(int page, int limit)
    {
        return await _carRepository.GetPaged(page, limit);
    }

    public async Task<List<CarWithMinInfoDto>> GetPagedCarsByClients (int page, int limit)
    {
        var car = await _carRepository.GetPaged(page, limit);
        var tariffId = car.Select(ca => ca.TariffId).FirstOrDefault();
        var statusId = car.Select(ca => ca.StatusId).FirstOrDefault();
        var categoryId = car.Select(ca => ca.CategoryId).FirstOrDefault();
        var specificationId = car.Select(c => c.SpecificationId).FirstOrDefault();

        var tariff = await _tariffRepository.GetById(tariffId);

        var status = await _statusRepository.GetById(statusId);

        var category = await _categoryRepository.GetById(categoryId);

        var specification = await _specificationCarRepository.GetById(specificationId);

        var response = (from c in car
            join t in tariff on c.TariffId equals t.Id
            join st in status on c.StatusId equals st.Id
            join sp in specification on c.SpecificationId equals sp.Id
            join cat in category on c.CategoryId equals cat.Id
            select new CarWithMinInfoDto(
                c.Id,
                st.Name,
                t.PricePerDay,
                cat.Name,
                sp.FuelType,
                $"{sp.Brand} {sp.Model}",
                sp.Transmission,
                c.ImagePath)).ToList();

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
        var statusId = car.Select(ca => ca.StatusId).FirstOrDefault();
        var categoryId = car.Select(ca => ca.CategoryId).FirstOrDefault();
        var specificationId = car.Select(c => c.SpecificationId).FirstOrDefault();

        var tariff = await _tariffRepository.GetById(tariffId);

        var status = await _statusRepository.GetById(statusId);

        var category = await _categoryRepository.GetById(categoryId);

        var specification = await _specificationCarRepository.GetById(specificationId);

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
        var statusId = car.Select(ca => ca.StatusId).FirstOrDefault();
        var categoryId = car.Select(ca => ca.CategoryId).FirstOrDefault();
        var specificationId = car.Select(c => c.SpecificationId).FirstOrDefault();

        var tariff = await _tariffRepository.GetById(tariffId);

        var status = await _statusRepository.GetById(statusId);

        var category = await _categoryRepository.GetById(categoryId);

        var specification = await _specificationCarRepository.GetById(specificationId);

        var response = (from c in car
            join t in tariff on c.TariffId equals t.Id
            join st in status on c.StatusId equals st.Id
            join sp in specification on c.SpecificationId equals sp.Id
            join cat in category on c.CategoryId equals cat.Id
            select new CarWithInfoAdminDto(
                c.Id,
                st.Name,
                cat.Name,
                sp.Transmission,
                $"{sp.Brand} {sp.Model}",
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
        var statusId = car.Select(ca => ca.StatusId).FirstOrDefault();
        var categoryId = car.Select(ca => ca.CategoryId).FirstOrDefault();
        var specificationId = car.Select(c => c.SpecificationId).FirstOrDefault();

        var tariff = await _tariffRepository.GetById(tariffId);

        var status = await _statusRepository.GetById(statusId);

        var category = await _categoryRepository.GetById(categoryId);

        var specification = await _specificationCarRepository.GetById(specificationId);

        var response = (from c in car
            join t in tariff on c.TariffId equals t.Id
            join st in status on c.StatusId equals st.Id
            join sp in specification on c.SpecificationId equals sp.Id
            join cat in category on c.CategoryId equals cat.Id
            select new CarWithMinInfoDto(
                c.Id,
                st.Name,
                t.PricePerDay,
                cat.Name,
                sp.FuelType,
                $"{sp.Brand} {sp.Model}",
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

    public async Task<int> UpdateCar(int id, int? statusId, int? tariffId, int? categoryId, int? specificationId,
        string? location, decimal? fuelLevel, string? imagePath)
    {
        return await _carRepository.Update(id, statusId, tariffId, categoryId, specificationId, location, fuelLevel, imagePath);
    }

    public async Task<int> DeleteCar(int id)
    {
        return await _carRepository.Delete(id);
    }
}