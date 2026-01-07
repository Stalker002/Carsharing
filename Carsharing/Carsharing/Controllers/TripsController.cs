using Carsharing.Application.DTOs;
using Carsharing.Application.Services;
using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Enum;
using Carsharing.Core.Models;
using Carsharing.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;
    private readonly ITripDetailsService _tripDetailsService;
    private readonly CarsharingDbContext _context;
    private readonly IClientsService _clientsService;
    private readonly ICarsService _carsService;
    private readonly IBookingsService _bookingsService;

    public TripsController(ITripService tripService, ITripDetailsService tripDetailsService, ICarsService carsService,
        CarsharingDbContext context, IClientsService clientsService, IBookingsService bookingsService)
    {
        _bookingsService = bookingsService;
        _carsService = carsService;
        _context = context;
        _clientsService = clientsService;
        _tripDetailsService = tripDetailsService;
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

        var clientClaim = await _clientsService.GetClientByUserId(userId);
        var client = clientClaim.FirstOrDefault();

        var (items, totalCount) = await _tripService.GetPagedHistoryByClientId(client!.Id, page, limit);

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

    [HttpPost("finish")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<TripFinishResult>> FinishTrip([FromBody] FinishTripRequest request)
    {
        try
        {
            var result = await _tripService.FinishTripAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    [Authorize(Policy = "AdminClientPolicy")]
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

            await _carsService.MarkCarAsUnavailableAsync(
                request.CarId
            );

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return Ok(tripId);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("cancel/{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> CancelTrip(int id)
    {
        try
        {
            await _tripService.CancelTripAsync(id);
            return Ok(new { message = "Поездка отменена" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> UpdateTrip(int id, [FromBody] TripRequest request)
    {
        var tripId = await _tripService.UpdateTrip(id, request.BookingId, request.StatusId, request.TariffType,
            request.StartTime, request.EndTime, request.Duration, request.Distance);

        if (request.EndTime == null && request.StatusId != (int)TripStatusEnum.Finished && request.StatusId != (int)TripStatusEnum.Cancelled) 
            return Ok(tripId);

        var booking = await _bookingsService.GetBookingsById(request.BookingId);
        var book = booking.FirstOrDefault();

        await _carsService.MarkCarAsAvailableAsync(
            book!.CarId
        );
        return Ok(tripId);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeleteTrip(int id)
    {
        return Ok(await _tripService.DeleteTrip(id));
    }
}