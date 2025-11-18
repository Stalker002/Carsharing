using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<List<CarsResponse>>> GetCars()
    {
        var cars = await _carsService.GetCars();
        var response = cars.Select(c =>
            new CarsResponse(c.Id, c.StatusId, c.TariffId, c.CategoryId, c.SpecificationId, c.Location, c.FuelLevel));

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateCar([FromBody] CarsRequest request)
    {
        var (cars, error) = Car.Create(
            0,
            request.StatusId,
            request.TariffId,
            request.CategoryId,
            request.SpecificationId,
            request.Location,
            request.FuelLevel);

        if (!string.IsNullOrWhiteSpace(error)) return BadRequest(error);

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