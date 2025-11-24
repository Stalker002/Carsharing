using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class StatusesController : ControllerBase
{
    private readonly IStatusesService _statusesService;

    public StatusesController(IStatusesService statusesService)
    {
        _statusesService = statusesService;
    }

    [HttpGet]
    public async Task<ActionResult<List<StatusResponse>>> GetStatuses()
    {
        var statuses = await _statusesService.GetStatuses();
        var response = statuses.Select(st => new StatusResponse(st.Id, st.Name, st.Description));

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<List<StatusResponse>>> GetStatusById(int id)
    {
        var statuses = await _statusesService.GetStatusById(id);
        var response = statuses.Select(st => new StatusResponse(st.Id, st.Name, st.Description));

        return Ok(response);
    }
}