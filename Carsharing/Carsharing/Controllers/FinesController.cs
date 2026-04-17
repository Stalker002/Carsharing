using Carsharing.Application.Abstractions;
using Carsharing.Core.Models;
using Carsharing.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Fines;

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
    public async Task<ActionResult<List<FinesResponse>>> GetFines(CancellationToken cancellationToken)
    {
        var fines = await _finesService.GetFines(cancellationToken);

        var response = fines.Select(f => new FinesResponse(f.Id, f.TripId, f.StatusId, f.Type, f.Amount, f.Date));

        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<FinesResponse>>> GetPagedFine(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25, CancellationToken cancellationToken = default)
    {
        var totalCount = await _finesService.GetCountFines(cancellationToken);
        var fines = await _finesService.GetPagedFines(page, limit, cancellationToken);

        var response = fines
            .Select(f => new FinesResponse(f.Id, f.TripId, f.StatusId, f.Type, f.Amount, f.Date)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<FinesResponse>>> GetFineById(int id, CancellationToken cancellationToken)
    {
        var fines = await _finesService.GetFineById(id, cancellationToken);

        var response = fines.Select(f => new FinesResponse(f.Id, f.TripId, f.StatusId, f.Type, f.Amount, f.Date));

        return Ok(response);
    }

    [HttpGet("byTrip/{tripId:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<FinesResponse>>> GetFinesByTripId(int tripId,
        CancellationToken cancellationToken)
    {
        var fines = User.IsAdmin()
            ? await _finesService.GetFinesByTripId(tripId, cancellationToken)
            : await _finesService.GetFinesByTripId(User.GetRequiredUserId(), tripId, cancellationToken);

        var response = fines.Select(f => new FinesResponse(f.Id, f.TripId, f.StatusId, f.Type, f.Amount, f.Date));

        return Ok(response);
    }

    [HttpGet("byStatus/{statusId:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<FinesResponse>>> GetFinesByStatusId(int statusId,
        CancellationToken cancellationToken)
    {
        var fines = await _finesService.GetFinesByStatusId(statusId, cancellationToken);

        var response = fines.Select(f => new FinesResponse(f.Id, f.TripId, f.StatusId, f.Type, f.Amount, f.Date));

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> CreateFine([FromBody] FinesRequest request,
        CancellationToken cancellationToken)
    {
        var (fines, error) = Fine.Create(
            0,
            request.TripId,
            request.StatusId,
            request.Type,
            request.Amount,
            request.Date);

        if (!string.IsNullOrWhiteSpace(error)) return BadRequest(error);

        var fineId = await _finesService.CreateFine(fines, cancellationToken);

        return Ok(fineId);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> UpdateFine(int id, [FromBody] FinesRequest request,
        CancellationToken cancellationToken)
    {
        var fineId = await _finesService.UpdateFine(id, request.TripId, request.StatusId, request.Type, request.Amount,
            request.Date, cancellationToken);
        return Ok(fineId);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeleteFine(int id, CancellationToken cancellationToken)
    {
        return Ok(await _finesService.DeleteFine(id, cancellationToken));
    }
}