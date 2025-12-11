using Carsharing.Application.DTOs;
using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess;
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
    private readonly IWebHostEnvironment _env;
    private readonly CarsharingDbContext _context;

    public CarsController(ICarsService carsService, ITariffsService tariffsService, ISpecificationsCarService specificationsCar, IWebHostEnvironment env, CarsharingDbContext context)
    {
        _context = context;
        _env = env;
        _specificationsCar = specificationsCar;
        _tariffsService = tariffsService;
        _carsService = carsService;
    }

    [HttpGet]
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
    public async Task<ActionResult<List<CarWithInfoDto>>> GetCarWithInfo(int id)
    {
        var carsWithInfo = await _carsService.GetCarWithInfo(id);
        var response = carsWithInfo.Select(c => new CarWithInfoDto(c.Id, c.StatusName, c.PricePerMinute, c.PricePerKm,
            c.PricePerDay, c.CategoryName, c.FuelType, c.Model, c.Transmission, c.Year, c.StateNumber, c.MaxFuel, c.Location, c.FuelLevel, c.ImagePath));

        return Ok(response);
    }

    [HttpGet("with-info-admin/{id:int}")]
    public async Task<ActionResult<List<CarWithInfoAdminDto>>> GetCarWithInfoAdmin(int id)
    {
        var carsWithInfo = await _carsService.GetCarWithInfoAdmin(id);
        var response = carsWithInfo.Select(c => new CarWithInfoAdminDto(c.Id, c.StatusName, c.CategoryName,
            c.Transmission, c.Model, c.Year, c.Location, c.VinNumber, c.StateNumber, c.FuelType, c.FuelLevel, c.MaxFuel,
            c.FuelPerKm, c.Mileage, c.TariffName, c.PricePerMinute, c.PricePerKm, c.PricePerDay, c.Image));

        return Ok(response);
    }

    [HttpGet("pagedByCategory")]
    public async Task<ActionResult<List<CarWithMinInfoDto>>> GetCarsByCategories([FromQuery] List<int>? ids,
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25)
    {
        if (ids == null || ids.Count == 0)
        {
            var totalCount = await _carsService.GetCount();
            var cars = await _carsService.GetPagedCarsByClients(page, limit);

            var response = cars
                .Select(c => new CarWithMinInfoDto(c.Id, c.StatusName, c.PricePerDay, c.CategoryName, c.FuelType, c.Model, c.Transmission, c.ImagePath)).ToList();

            Response.Headers.Append("x-total-count", totalCount.ToString());

            return Ok(response);
        }
        else
        {
            var totalCount = await _carsService.GetCountByCategory(ids);
            var cars = await _carsService.GetCarWithMinInfoByCategoryIds(ids, page, limit);

            var response = cars.Select(c =>
                new CarWithMinInfoDto(c.Id, c.StatusName, c.PricePerDay, c.CategoryName, c.FuelType, c.Model, c.Transmission, c.ImagePath));

            Response.Headers.Append("x-total-count", totalCount.ToString());

            return Ok(response);
        }
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<int>> CreateCar([FromForm] CarsCreateRequest request)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        string? createdImagePath = null;

        try
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
                return BadRequest(errorSpecification);
            }

            var specificationId = await _specificationsCar.CreateSpecification(specification);

            string? imagePath = null;

            if (request.Image is { Length: > 0 })
            {
                var ext = Path.GetExtension(request.Image.FileName).ToLowerInvariant();
                var allowed = new[] { ".jpg", ".jpeg", ".png" };

                if (!allowed.Contains(ext))
                {
                    return BadRequest("Unsupported image format");
                }

                var fileName = $"car_{Guid.NewGuid()}{ext}";
                var rootPath = _env.WebRootPath;

                if (string.IsNullOrEmpty(rootPath))
                {
                    return StatusCode(500, "Не удалось определить корневой путь для загрузки файлов.");
                }

                var folder = Path.Combine(rootPath, "images", "cars");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                var path = Path.Combine(folder, fileName);

                await using (var stream = new FileStream(path, FileMode.Create))
                {
                    await request.Image.CopyToAsync(stream);
                }

                imagePath = $"/images/cars/{fileName}";
                createdImagePath = path;
            }

            var (car, error) = Car.Create(
                0,
                request.StatusId,
                tariffId,
                request.CategoryId,
                specificationId,
                request.Location,
                request.FuelLevel,
                imagePath);

            if (!string.IsNullOrWhiteSpace(error))
            {
                if (createdImagePath != null && System.IO.File.Exists(createdImagePath))
                {
                    System.IO.File.Delete(createdImagePath);
                }
                return BadRequest(error);
            }

            var carId = await _carsService.CreateCar(car);

            await transaction.CommitAsync();

            return Ok(carId);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            if (createdImagePath != null && System.IO.File.Exists(createdImagePath))
            {
                try { System.IO.File.Delete(createdImagePath); }
                catch
                {
                    // ignored
                }
            }

            if (ex.InnerException != null && ex.InnerException.Message.Contains("23505"))
            {
                if (ex.InnerException.Message.Contains("state_number"))
                    return BadRequest("Автомобиль с таким Гос. номером уже существует.");

                if (ex.InnerException.Message.Contains("vin_number"))
                    return BadRequest("Автомобиль с таким VIN уже существует.");
            }

            var errorMessage = ex.InnerException != null
                ? $"{ex.Message} -> {ex.InnerException.Message}"
                : ex.Message;

            return StatusCode(500, new { error = "Ошибка сервера", details = errorMessage });
        }
    }

    [HttpPut("{id:int}")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<int>> UpdateCar(int id, [FromForm] CarsRequest request)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        string? createdImagePath = null;

        try
        {
            var carsList = await _carsService.GetCarById(id);
            var car = carsList.FirstOrDefault();

            if (car == null) return NotFound("Car not found");

            var imagePathForDb = car.ImagePath;

            if (request.Image is { Length: > 0 })
            {
                var ext = Path.GetExtension(request.Image.FileName).ToLowerInvariant();
                var allowed = new[] { ".jpg", ".jpeg", ".png" };

                if (!allowed.Contains(ext)) return BadRequest("Unsupported image format");

                var fileName = $"{Guid.NewGuid()}{ext}";

                var rootPath = _env.WebRootPath;
                var folder = Path.Combine(rootPath, "images", "cars");

                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                var fullPath = Path.Combine(folder, fileName);

                await using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await request.Image.CopyToAsync(stream);
                }

                createdImagePath = fullPath; 
                imagePathForDb = $"/images/cars/{fileName}";
            }

            await _carsService.UpdateCar(
                id,
                request.StatusId,
                request.TariffId,
                request.CategoryId,
                request.SpecificationId,
                request.Location,
                request.FuelLevel,
                imagePathForDb 
            );

            await transaction.CommitAsync();

            return Ok(id);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            if (createdImagePath != null && System.IO.File.Exists(createdImagePath))
            {
                try { System.IO.File.Delete(createdImagePath); }
                catch
                {
                    // ignored
                }
            }

            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<int>> DeleteCar(int id)
    {
        return Ok(await _carsService.DeleteCar(id));
    }
}