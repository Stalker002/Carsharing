using Carsharing.Application.DTOs;
using Carsharing.Application.Services;
using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;
    private readonly ITripDetailsService _tripDetailsService;
    private readonly CarsharingDbContext _context;

    public TripsController(ITripService tripService, ITripDetailsService tripDetailsService, CarsharingDbContext context)
    {
        _context = context;
        _tripDetailsService = tripDetailsService;
        _tripService = tripService;
    }

    [HttpGet("unpaged")]
    public async Task<ActionResult<List<TripResponse>>> GetTrips()
    {
        var trips = await _tripService.GetTrips();

        var response = trips.Select(tr => new TripResponse(tr.Id, tr.BookingId, tr.StatusId, tr.TariffType,
            tr.StartTime, tr.EndTime, tr.Duration, tr.Distance));

        return Ok(response);
    }

    [HttpGet]
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

    [HttpGet("pagedByClient")]
    public async Task<ActionResult<List<TripWithMinInfoDto>>> GetPagedTripsByClient(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25)
    {
        var userId = int.Parse(User.FindFirst("userId")!.Value);

        var totalCount = await _tripService.GetCountPagedBillWithMinInfoByUser(userId);
        var trips = await _tripService.GetPagedTripWithMinInfoByUserId(userId, page, limit);

        var response = trips
            .Select(tr => new TripWithMinInfoDto(tr.Id, tr.BookingId, tr.StatusName, tr.TariffType, tr.StartTime,
                tr.EndTime, tr.Duration, tr.Distance)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<List<TripWithInfoDto>>> GetTripWithInfo(int id)
    {
        var tripWithInfo = await _tripService.GetTripWithInfo(id);
        var response = tripWithInfo.Select(t => new TripWithInfoDto(t.Id, t.BookingId, t.StatusName, t.StartLocation,
            t.EndLocation, t.InsuranceActive, t.FuelUsed, t.Refueled, t.TariffType, t.StartTime, t.EndTime, t.Duration,
            t.Distance));

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateTrip([FromBody] TripCreateRequest request)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
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

            if (!string.IsNullOrWhiteSpace(error))
            {
                return BadRequest(error);
            }

           
            var tripId = await _tripService.CreateTrip(trip);

            var (tripDetail, errorTripDetail) = TripDetail.Create(
                0,
                tripId,
                request.StartLocation,
                request.EndLocation,
                request.InsuranceActive,
                request.FuelUsed,
                request.Refueled);

            if (!string.IsNullOrWhiteSpace(errorTripDetail))
            {
                await transaction.RollbackAsync();
                return BadRequest(errorTripDetail);
            }

            await _tripDetailsService.CreateTripDetail(tripDetail);

            await transaction.CommitAsync();

            return Ok(tripId);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
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