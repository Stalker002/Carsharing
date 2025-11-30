using Carsharing.Application.DTOs;
using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
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

    [HttpGet("unpaged")]
    public async Task<ActionResult<List<CarsResponse>>> GetCars()
    {
        var cars = await _carsService.GetCars();
        var response = cars.Select(c =>
            new CarsResponse(c.Id, c.StatusId, c.TariffId, c.CategoryId, c.SpecificationId, c.Location, c.FuelLevel));

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CarsResponse>>> GetPagedCars(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25)
    {
        var totalCount = await _carsService.GetCount();
        var cars = await _carsService.GetPagedCars(page, limit);

        var response = cars
            .Select(c => new CarsResponse(c.Id, c.StatusId, c.TariffId, c.CategoryId, c.SpecificationId, c.Location, c.FuelLevel)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("with-info/{id:int}")]
    public async Task<ActionResult<List<CarWithInfoDto>>> GetCarWithInfo(int id)
    {
        var carsWithInfo = await _carsService.GetCarWithInfo(id);
        var response = carsWithInfo.Select(c => new CarWithInfoDto(c.Id, c.StatusName, c.PricePerMinute, c.PricePerKm,
            c.PricePerDay, c.CategoryName, c.FuelType, c.Model, c.Transmission, c.Year,c.StateNumber, c.MaxFuel, c.Location, c.FuelLevel));

        return Ok(response);
    }

    [HttpGet("with-minInfo/{id:int}")]
    public async Task<ActionResult<List<CarWithInfoDto>>> GetCarWithMinInfo(int id)
    {
        var carsWithMinInfo = await _carsService.GetCarWithMinInfo(id);
        var response = carsWithMinInfo.Select(c => new CarWithMinInfoDto(c.Id, c.StatusName,
            c.PricePerDay, c.CategoryName, c.FuelType, c.Model, c.Transmission));

        return Ok(response);
    }

    [HttpGet("byCategory")]
    public async Task<ActionResult<List<CarsResponse>>> GetCarsByCategories([FromQuery] List<int>? ids)
    {
        if (ids == null || ids.Count == 0)
            return BadRequest("Category IDs are required.");

        var cars = await _carsService.GetCarsByCategoryIds(ids);

        var response = cars.Select(c =>
            new CarsResponse(c.Id, c.StatusId, c.TariffId, c.CategoryId, c.SpecificationId, c.Location, c.FuelLevel));

        return Ok(response);
    }

    [HttpGet("pagedByCategory")]
    public async Task<ActionResult<List<CarsResponse>>> GetPagedCarsByCategories(
        [FromQuery] List<int>? ids,
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25)
    {

        if (ids == null || ids.Count == 0)
            return BadRequest("Category IDs are required.");

        var totalCount = await _carsService.GetCountByCategory(ids);
        var reviews = await _carsService.GetPagedCarsByCategoryIds(ids, page, limit);

        var response = reviews
            .Select(c => new CarsResponse(c.Id, c.StatusId, c.TariffId, c.CategoryId, c.SpecificationId, c.Location, c.FuelLevel)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<int>> CreateCar([FromBody] CarsCreateRequest request, [FromForm] IFormFile image)
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

        if (!string.IsNullOrWhiteSpace(errorSpecification))
        {
            await _tariffsService.DeleteTariff(tariffId);
            return BadRequest(errorSpecification);
        }

        var specificationId = await _specificationsCar.CreateSpecification(specification);

        var (cars, error) = Car.Create(
            0,
            request.StatusId,
            tariffId,
            request.CategoryId,
            specificationId,
            request.Location,
            request.FuelLevel,
            null);

        if (!string.IsNullOrWhiteSpace(error))
        {
            await _specificationsCar.DeleteSpecification(specificationId);
            await _tariffsService.DeleteTariff(tariffId);
            return BadRequest(error);
        }

        var carId = await _carsService.CreateCar(cars);

        return Ok(carId);
    }

    [HttpPost("{id:int}/image")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadCarImage(
        int id,
        [FromForm] IFormFile? image)
    {
        if (image == null)
            return BadRequest("Image is required.");

        var car = await _carsService.GetCarById(id);

        var ext = Path.GetExtension(image.FileName);
        var fileName = $"{Guid.NewGuid()}{ext}";
        var saveDir = Path.Combine("wwwroot", "images", "cars");

        Directory.CreateDirectory(saveDir);

        var filePath = Path.Combine(saveDir, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        car.ImagePath = $"/images/cars/{fileName}";
        await _carsService.UpdateCar(id);

        return Ok(new { imageUrl = car.ImagePath });
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