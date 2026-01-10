using Carsharing.Application.Abstractions;
using Carsharing.Application.DTOs;
using Carsharing.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<List<TripResponse>>> GetTrips()
    {
        var trips = await _tripService.GetTrips();

        var response = trips.Select(tr => new TripResponse(tr.Id, tr.BookingId, tr.StatusId, tr.TariffType,
            tr.StartTime, tr.EndTime, tr.Duration, tr.Distance));

        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<TripResponse>>> GetPagedTrips(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25)
    {
        var totalCount = await _tripService.GetCountTrips();
        var trips = await _tripService.GetPagedTrips(page, limit);

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
        [FromQuery] int limit = 10)
    {
        var userId = int.Parse(User.FindFirst("userId")!.Value);

        var (items, totalCount) = await _tripService.GetPagedHistoryByUserId(userId, page, limit);

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(items);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<TripWithInfoDto>>> GetTripWithInfo(int id)
    {
        var tripWithInfo = await _tripService.GetTripWithInfo(id);
        var response = tripWithInfo.Select(t => new TripWithInfoDto(t.Id, t.BookingId, t.StatusId, t.StartLocation,
            t.EndLocation, t.InsuranceActive, t.FuelUsed, t.Refueled, t.TariffType, t.StartTime, t.EndTime, t.Duration,
            t.Distance));

        return Ok(response);
    }

    [HttpGet("current")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<CurrentTripDto>> GetCurrentTrip()
    {
        var userId = int.Parse(User.FindFirst("userId")!.Value);

        var trip = await _tripService.GetActiveTripByClientId(userId);
        if (trip == null)
            return NotFound(new { message = "Активных поездок нет" });

        return Ok(trip);
    }

    [HttpPost]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<int>> CreateTrip([FromBody] TripCreateRequest request)
    {
        var tripId = await _tripService.CreateTripAsync(request);
        return Ok(tripId);
    }

    [HttpPost("finish")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<TripFinishResult>> FinishTrip([FromBody] FinishTripRequest request)
    {
        var result = await _tripService.FinishTripAsync(request);
        return Ok(result);
    }

    [HttpPost("cancel/{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> CancelTrip(int id)
    {
        await _tripService.CancelTripAsync(id);
        return Ok(new { message = "Поездка отменена" });
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> UpdateTrip(int id, [FromBody] TripUpdateRequest request)
    {
        var tripId = await _tripService.UpdateTrip(id, request);
        return Ok(tripId);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeleteTrip(int id)
    {
        return Ok(await _tripService.DeleteTrip(id));
    }
}