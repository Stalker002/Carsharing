using Carsharing.Application.DTOs;
using Carsharing.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ICarsService = Carsharing.Application.Services.ICarsService;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class CarsController : ControllerBase
{
    private readonly ICarsService _carsService;

    public CarsController(ICarsService carsService)
    {
        _carsService = carsService;
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<CarsResponse>>> GetPagedCars(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25)
    {
        var totalCount = await _carsService.GetCount();
        var cars = await _carsService.GetPagedCars(page, limit);

        var response = cars
            .Select(c => new CarsResponse(c.Id, c.StatusId, c.TariffId, c.CategoryId, c.SpecificationId, c.Location, c.FuelLevel, c.ImagePath)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("with-info/{id:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<CarWithInfoDto>>> GetCarWithInfo(int id)
    {
        var carsWithInfo = await _carsService.GetCarWithInfo(id);
        var response = carsWithInfo.Select(c => new CarWithInfoDto(c.Id, c.StatusName, c.PricePerMinute, c.PricePerKm,
            c.PricePerDay, c.CategoryName, c.FuelType, c.Brand, c.Model, c.Transmission, c.Year, c.StateNumber, c.MaxFuel, c.Location, c.FuelLevel, c.ImagePath));

        return Ok(response);
    }

    [HttpGet("with-info-admin/{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<CarWithInfoAdminDto>>> GetCarWithInfoAdmin(int id)
    {
        var carsWithInfo = await _carsService.GetCarWithInfoAdmin(id);
        var response = carsWithInfo.Select(c => new CarWithInfoAdminDto(c.Id, c.StatusId, c.CategoryId,
            c.Transmission, c.Brand, c.Model, c.Year, c.Location, c.VinNumber, c.StateNumber, c.FuelType, c.FuelLevel, c.MaxFuel,
            c.FuelPerKm, c.Mileage, c.TariffName, c.PricePerMinute, c.PricePerKm, c.PricePerDay, c.Image));

        return Ok(response);
    }

    [HttpGet("pagedByCategory")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<CarWithMinInfoDto>>> GetCarsByCategories([FromQuery] List<int>? ids,
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25)
    {
        if (ids == null || ids.Count == 0)
        {
            var totalCount = await _carsService.GetCount();
            var cars = await _carsService.GetPagedCarsByClients(page, limit);

            var response = cars
                .Select(c => new CarWithMinInfoDto(c.Id, c.StatusName, c.PricePerDay, c.CategoryName, c.FuelType, c.MaxFuel, c.Brand, c.Model, c.Transmission, c.ImagePath)).ToList();

            Response.Headers.Append("x-total-count", totalCount.ToString());

            return Ok(response);
        }
        else
        {
            var totalCount = await _carsService.GetCountByCategory(ids);
            var cars = await _carsService.GetCarWithMinInfoByCategoryIds(ids, page, limit);

            var response = cars.Select(c =>
                new CarWithMinInfoDto(c.Id, c.StatusName, c.PricePerDay, c.CategoryName, c.FuelType, c.MaxFuel, c.Brand, c.Model, c.Transmission, c.ImagePath));

            Response.Headers.Append("x-total-count", totalCount.ToString());

            return Ok(response);
        }
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> CreateCar([FromForm] CarsCreateRequest request)
    {
        var (carId, error) = await _carsService.CreateCarFullAsync(request);

        if (string.IsNullOrEmpty(error)) return Ok(carId);

        return error.Contains("Internal error") ? StatusCode(500, error) : BadRequest(error);
    }

    [HttpPut("{id:int}")]
    [Consumes("multipart/form-data")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> UpdateCar(int id, [FromForm] CarUpdateDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (isSuccess, error) = await _carsService.UpdateCarFullAsync(id, request);

        if (isSuccess) return Ok(id);

        return error == "Car not found" ? NotFound(error) : StatusCode(500, new { error });
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeleteCar(int id)
    {
        return Ok(await _carsService.DeleteCar(id));
    }
}