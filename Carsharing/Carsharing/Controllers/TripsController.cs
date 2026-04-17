using Carsharing.Application.Abstractions;
using Carsharing.Contracts;
using Carsharing.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Trip;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;

    public TripsController(ITripService tripService)
    {
        _tripService = tripService;
    }

    [HttpGet("unpaged")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<TripResponse>>> GetTrips(CancellationToken cancellationToken)
    {
        var trips = await _tripService.GetTrips(cancellationToken);

        var response = trips.Select(tr => new TripResponse(tr.Id, tr.BookingId, tr.StatusId, tr.TariffType,
            tr.StartTime, tr.EndTime, tr.Duration, tr.Distance));

        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<TripResponse>>> GetPagedTrips(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25, CancellationToken cancellationToken = default)
    {
        var totalCount = await _tripService.GetCountTrips(cancellationToken);
        var trips = await _tripService.GetPagedTrips(page, limit, cancellationToken);

        var response = trips
            .Select(tr => new TripResponse(tr.Id, tr.BookingId, tr.StatusId, tr.TariffType,
                tr.StartTime, tr.EndTime, tr.Duration, tr.Distance)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("history")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<TripHistoryDto>>> GetMyHistory(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10, CancellationToken cancellationToken = default)
    {
        var userId = User.GetRequiredUserId();

        var (items, totalCount) = await _tripService.GetPagedHistoryByUserId(userId, page, limit, cancellationToken);

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(items);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<TripWithInfoDto>>> GetTripWithInfo(int id, CancellationToken cancellationToken)
    {
        var tripWithInfo = User.IsAdmin()
            ? await _tripService.GetTripWithInfo(id, cancellationToken)
            : await _tripService.GetTripWithInfo(User.GetRequiredUserId(), id, cancellationToken);
        var response = tripWithInfo.Select(t => new TripWithInfoDto(t.Id, t.BookingId, t.StatusId, t.StartLocation,
            t.EndLocation, t.InsuranceActive, t.FuelUsed, t.Refueled, t.TariffType, t.StartTime, t.EndTime, t.Duration,
            t.Distance));

        return Ok(response);
    }

    [HttpGet("current")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<CurrentTripDto>> GetCurrentTrip(CancellationToken cancellationToken)
    {
        var userId = User.GetRequiredUserId();

        var trip = await _tripService.GetActiveTripByClientId(userId, cancellationToken);
        if (trip == null)
            return NotFound(new { message = "Активных поездок нет" });

        return Ok(trip);
    }

    [HttpPost]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<int>> CreateTrip([FromBody] TripCreateRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.GetRequiredUserId();
        var tripId = await _tripService.CreateTripAsync(userId, request, cancellationToken);
        return Ok(tripId);
    }

    [HttpPut("{id:int}/location")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<IActionResult> UpdateTripLocation(int id, [FromBody] UpdateTripLocationRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.GetRequiredUserId();

        await _tripService.UpdateTripLocationAsync(
            userId,
            id,
            request.Location,
            request.CarLatitude,
            request.CarLongitude,
            cancellationToken);

        return Ok(new { message = "Позиция поездки обновлена" });
    }

    [HttpPost("finish")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<TripFinishResult>> FinishTrip([FromBody] FinishTripRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.GetRequiredUserId();
        var result = await _tripService.FinishTripAsync(userId, request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("cancel/{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> CancelTrip(int id, CancellationToken cancellationToken)
    {
        await _tripService.CancelTripAsync(id, cancellationToken);
        return Ok(new { message = "Поездка отменена" });
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> UpdateTrip(int id, [FromBody] TripUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var tripId = await _tripService.UpdateTrip(id, request, cancellationToken);
        return Ok(tripId);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeleteTrip(int id, CancellationToken cancellationToken)
    {
        return Ok(await _tripService.DeleteTrip(id, cancellationToken));
    }
}
