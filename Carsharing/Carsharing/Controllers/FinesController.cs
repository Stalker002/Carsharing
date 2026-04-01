using Carsharing.Application.Abstractions;
using Carsharing.Contracts;
using Carsharing.Core.Models;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("unpaged")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<FinesResponse>>> GetFines()
    {
        var fines = await _finesService.GetFines();

        var response = fines.Select(f => new FinesResponse(f.Id, f.TripId, f.StatusId, f.Type, f.Amount, f.Date));

        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<FinesResponse>>> GetPagedFine(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25)
    {
        var totalCount = await _finesService.GetCountFines();
        var fines = await _finesService.GetPagedFines(page, limit);

        var response = fines
            .Select(f => new FinesResponse(f.Id, f.TripId, f.StatusId, f.Type, f.Amount, f.Date)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<FinesResponse>>> GetFineById(int id)
    {
        var fines = await _finesService.GetFineById(id);

        var response = fines.Select(f => new FinesResponse(f.Id, f.TripId, f.StatusId, f.Type, f.Amount, f.Date));

        return Ok(response);
    }

    [HttpGet("byTrip/{tripId:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<FinesResponse>>> GetFinesByTripId(int tripId)
    {
        var fines = await _finesService.GetFinesByTripId(tripId);

        var response = fines.Select(f => new FinesResponse(f.Id, f.TripId, f.StatusId, f.Type, f.Amount, f.Date));

        return Ok(response);
    }

    [HttpGet("byStatus/{statusId:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<FinesResponse>>> GetFinesByStatusId(int statusId)
    {
        var fines = await _finesService.GetFinesByStatusId(statusId);

        var response = fines.Select(f => new FinesResponse(f.Id, f.TripId, f.StatusId, f.Type, f.Amount, f.Date));

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
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
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> UpdateFine(int id, [FromBody] FinesRequest request)
    {
        var fineId = await _finesService.UpdateFine(id, request.TripId, request.StatusId, request.Type, request.Amount,
            request.Date);
        return Ok(fineId);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeleteFine(int id)
    {
        return Ok(await _finesService.DeleteFine(id));
    }
}