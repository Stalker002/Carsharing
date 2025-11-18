using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class FinesController : ControllerBase
{
    private readonly IFinesService _finesService;

    public FinesController(IFinesService finesService)
    {
        _finesService = finesService;
    }

    [HttpGet]
    public async Task<ActionResult<List<FinesResponse>>> GetFines()
    {
        var fines = await _finesService.GetFines();

        var response = fines.Select(f => new FinesResponse(f.Id, f.TripId, f.StatusId, f.Type, f.Amount, f.Date));

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateFine([FromBody] FinesRequest request)
    {
        var (fines, error) = Fine.Create(
            0,
            request.TripId,
            request.StatusId,
            request.Type,
            request.Amount,
            request.Date);

        if (!string.IsNullOrWhiteSpace(error)) return BadRequest(error);

        var fineId = await _finesService.CreateFine(fines);

        return Ok(fineId);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<int>> UpdateFine(int id, [FromBody] FinesRequest request)
    {
        var fineId = await _finesService.UpdateFine(id, request.TripId, request.StatusId, request.Type, request.Amount,
            request.Date);
        return Ok(fineId);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<int>> DeleteFine(int id)
    {
        return Ok(await _finesService.DeleteFine(id));
    }
}