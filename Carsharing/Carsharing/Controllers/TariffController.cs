using Carsharing.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Statuses;
using Shared.Contracts.Tariff;

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
    public async Task<ActionResult<List<StatusResponse>>> GetTariffs(CancellationToken cancellationToken = default)
    {
        var statuses = await _tariffsService.GetTariffs(cancellationToken);
        var response = statuses.Select(t =>
            new TariffResponse(t.Id, t.Name, t.PricePerMinute, t.PricePerKm, t.PricePerDay));

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<StatusResponse>>> GetTariffById(int id,
        CancellationToken cancellationToken = default)
    {
        var statuses = await _tariffsService.GetTariffById(id, cancellationToken);
        var response = statuses.Select(t =>
            new TariffResponse(t.Id, t.Name, t.PricePerMinute, t.PricePerKm, t.PricePerDay));

        return Ok(response);
    }
}