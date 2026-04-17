using Carsharing.Application.Abstractions;
using Carsharing.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Maintenance;

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
    public async Task<ActionResult<List<MaintenanceResponse>>> GetMaintenances(CancellationToken cancellationToken)
    {
        var maintenances = await _maintenancesService.GetMaintenances(cancellationToken);
        var response = maintenances.Select(m =>
            new MaintenanceResponse(m.Id, m.CarId, m.WorkType, m.Description, m.Cost, m.Date));
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<MaintenanceResponse>>> GetMaintenanceById(int id,
        CancellationToken cancellationToken)
    {
        var maintenances = await _maintenancesService.GetMaintenanceById(id, cancellationToken);
        var response = maintenances.Select(m =>
            new MaintenanceResponse(m.Id, m.CarId, m.WorkType, m.Description, m.Cost, m.Date));
        return Ok(response);
    }

    [HttpGet("byCar/{carId:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<MaintenanceResponse>>> GetMaintenanceByCarId(int carId,
        CancellationToken cancellationToken)
    {
        var maintenances = await _maintenancesService.GetMaintenanceByCarId(carId, cancellationToken);
        var response = maintenances.Select(m =>
            new MaintenanceResponse(m.Id, m.CarId, m.WorkType, m.Description, m.Cost, m.Date));
        return Ok(response);
    }

    [HttpGet("byDate")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<MaintenanceResponse>>> GetMaintenanceByDateRange([FromQuery] DateOnly from,
        [FromQuery] DateOnly to, CancellationToken cancellationToken)
    {
        if (from > to)
            return BadRequest("Дата 'from' не может быть больше 'to'.");

        var maintenances = await _maintenancesService.GetByDateRange(from, to, cancellationToken);

        var response = maintenances.Select(m =>
            new MaintenanceResponse(m.Id, m.CarId, m.WorkType, m.Description, m.Cost, m.Date));
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> CreateMaintenance([FromBody] MaintenanceRequest request,
        CancellationToken cancellationToken)
    {
        var (maintenance, error) = Maintenance.Create(
            0,
            request.CarId,
            request.WorkType,
            request.Description,
            request.Cost,
            request.Date);

        if (!string.IsNullOrWhiteSpace(error)) return BadRequest(error);

        var maintenanceId = await _maintenancesService.CreateMaintenance(maintenance, cancellationToken);

        return Ok(maintenanceId);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> UpdateMaintenance(int id, [FromBody] MaintenanceRequest request,
        CancellationToken cancellationToken)
    {
        var maintenanceId = await _maintenancesService.UpdateMaintenance(id, request.CarId, request.WorkType,
            request.Description, request.Cost, request.Date, cancellationToken);
        return Ok(maintenanceId);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeleteMaintenance(int id, CancellationToken cancellationToken)
    {
        return Ok(await _maintenancesService.DeleteMaintenance(id, cancellationToken));
    }
}