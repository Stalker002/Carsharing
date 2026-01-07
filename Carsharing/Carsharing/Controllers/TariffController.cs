using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class TariffController : ControllerBase
{
    private readonly ITariffsService _tariffsService;

    public TariffController(ITariffsService tariffsService)
    {
        _tariffsService = tariffsService;
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<StatusResponse>>> GetTariffs()
    {
        var statuses = await _tariffsService.GetTariffs();
        var response = statuses.Select(t =>
            new TariffResponse(t.Id, t.Name, t.PricePerMinute, t.PricePerKm, t.PricePerDay));

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<StatusResponse>>> GetTariffById(int id)
    {
        var statuses = await _tariffsService.GetTariffById(id);
        var response = statuses.Select(t =>
            new TariffResponse(t.Id, t.Name, t.PricePerMinute, t.PricePerKm, t.PricePerDay));

        return Ok(response);
    }
}