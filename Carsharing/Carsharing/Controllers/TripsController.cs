using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
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

    [HttpGet]
    public async Task<ActionResult<List<TripResponse>>> GetTrip()
    {
        var trips = await _tripService.GetTrips();

        var response = trips.Select(tr => new TripResponse(tr.Id, tr.BookingId, tr.StatusId, tr.TariffType,
            tr.StartTime, tr.EndTime, tr.Duration, tr.Distance));

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateTrip([FromBody] TripRequest request)
    {
        var (trip, error) = Trip.Create(
            0,
            request.BookingId,
            request.StatusId,
            request.TariffType,
            request.StartTime,
            request.EndTime,
            request.Duration,
            request.Distance);

        if (!string.IsNullOrWhiteSpace(error)) return BadRequest(error);

        var tripId = await _tripService.CreateTrip(trip);

        return Ok(tripId);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<int>> UpdateTrip(int id, [FromBody] TripRequest request)
    {
        var tripId = await _tripService.UpdateTrip(id, request.BookingId, request.StatusId, request.TariffType,
            request.StartTime, request.EndTime, request.Duration, request.Distance);

        return Ok(tripId);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<int>> DeleteTrip(int id)
    {
        return Ok(await _tripService.DeleteTrip(id));
    }
}