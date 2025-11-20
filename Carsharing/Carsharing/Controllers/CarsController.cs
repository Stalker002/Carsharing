using Carsharing.Application.DTOs;
using Carsharing.Application.Services;
using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Microsoft.AspNetCore.Mvc;
using ICarsService = Carsharing.Application.Services.ICarsService;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class CarsController : ControllerBase
{
    private readonly ICarsService _carsService;
    private readonly ITariffsService _tariffsService;
    private readonly ISpecificationsCarService _specificationsCar;

    public CarsController(ICarsService carsService, ITariffsService tariffsService, ISpecificationsCarService specificationsCar)
    {
        _specificationsCar = specificationsCar;
        _tariffsService = tariffsService;
        _carsService = carsService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CarsResponse>>> GetCars()
    {
        var cars = await _carsService.GetCars();
        var response = cars.Select(c =>
            new CarsResponse(c.Id, c.StatusId, c.TariffId, c.CategoryId, c.SpecificationId, c.Location, c.FuelLevel));

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<List<CarWithInfoDto>>> GetCarWithInfo(int id)
    {
        var response = await _carsService.GetCarWithInfo(id);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateCar([FromBody] CarsCreateRequest request)
    {
        var (tariff, errorTariff) = Tariff.Create(
            0,
            request.Name,
            request.PricePerMinute,
            request.PricePerKm,
            request.PricePerDay);

        if (!string.IsNullOrWhiteSpace(errorTariff)) return BadRequest(errorTariff);

        var tariffId = await _tariffsService.CreateTariff(tariff);

        var (specification, errorSpecification) = SpecificationCar.Create(
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

        if (!string.IsNullOrWhiteSpace(errorSpecification)) return BadRequest(errorSpecification);

        var specificationId = await _specificationsCar.CreateSpecification(specification);

        var (cars, error) = Car.Create(
            0,
            request.StatusId,
            tariffId,
            request.CategoryId,
            specificationId,
            request.Location,
            request.FuelLevel);

        if (!string.IsNullOrWhiteSpace(error))
        {
            await _specificationsCar.DeleteSpecification(specificationId);
            await _tariffsService.DeleteTariff(tariffId);
            return BadRequest(error);
        }

        var carId = await _carsService.CreateCar(cars);

        return Ok(carId);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<int>> UpdateCar(int id, [FromBody] CarsRequest request)
    {
        var carId = await _carsService.UpdateCar(id, request.StatusId, request.TariffId, request.CategoryId,
            request.SpecificationId, request.Location, request.FuelLevel);
        return Ok(carId);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<int>> DeleteCar(int id)
    {
        return Ok(await _carsService.DeleteCar(id));
    }
}