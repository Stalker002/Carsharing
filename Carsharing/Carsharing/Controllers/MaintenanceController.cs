using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class MaintenanceController : ControllerBase
{
    private readonly IMaintenancesService _maintenancesService;

    public MaintenanceController(IMaintenancesService maintenancesService)
    {
        _maintenancesService = maintenancesService;
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<MaintenanceResponse>>> GetMaintenances()
    {
        var maintenances = await _maintenancesService.GetMaintenances();
        var response = maintenances.Select(m =>
            new MaintenanceResponse(m.Id, m.CarId, m.WorkType, m.Description, m.Cost, m.Date));
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<MaintenanceResponse>>> GetMaintenanceById(int id)
    {
        var maintenances = await _maintenancesService.GetMaintenanceById(id);
        var response = maintenances.Select(m =>
            new MaintenanceResponse(m.Id, m.CarId, m.WorkType, m.Description, m.Cost, m.Date));
        return Ok(response);
    }

    [HttpGet("byCar/{carId:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<MaintenanceResponse>>> GetMaintenanceByCarId(int carId)
    {
        var maintenances = await _maintenancesService.GetMaintenanceByCarId(carId);
        var response = maintenances.Select(m =>
            new MaintenanceResponse(m.Id, m.CarId, m.WorkType, m.Description, m.Cost, m.Date));
        return Ok(response);
    }

    [HttpGet("byDate")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<MaintenanceResponse>>> GetMaintenanceByDateRange([FromQuery] DateOnly from,
        [FromQuery] DateOnly to)
    {
        if (from > to)
            return BadRequest("Дата 'from' не может быть больше 'to'.");

        var maintenances = await _maintenancesService.GetByDateRange(from, to);

        var response = maintenances.Select(m =>
            new MaintenanceResponse(m.Id, m.CarId, m.WorkType, m.Description, m.Cost, m.Date));
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> CreateMaintenance([FromBody] MaintenanceRequest request)
    {
        var (maintenance, error) = Maintenance.Create(
            0,
            request.CarId,
            request.WorkType,
            request.Description,
            request.Cost,
            request.Date);

        if (!string.IsNullOrWhiteSpace(error)) return BadRequest(error);

        var maintenanceId = await _maintenancesService.CreateMaintenance(maintenance);

        return Ok(maintenanceId);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> UpdateMaintenance(int id, [FromBody] MaintenanceRequest request)
    {
        var maintenanceId = await _maintenancesService.UpdateMaintenance(id, request.CarId, request.WorkType,
            request.Description, request.Cost, request.Date);
        return Ok(maintenanceId);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeleteMaintenance(int id)
    {
        return Ok(await _maintenancesService.DeleteMaintenance(id));
    }
}