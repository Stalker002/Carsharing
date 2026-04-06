using Carsharing.Application.Abstractions;
using Carsharing.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Bookings;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingsService _bookingsService;

    public BookingsController(IBookingsService bookingsService)
    {
        _bookingsService = bookingsService;
    }

    [HttpGet("unpaged")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<BookingsResponse>>> GetBookings(CancellationToken cancellationToken)
    {
        var bookings = await _bookingsService.GetBookings(cancellationToken);
        var response = bookings.Select(b =>
            new BookingsResponse(b.Id, b.StatusId, b.CarId, b.ClientId, b.StartTime, b.EndTime));

        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<BookingsResponse>>> GetPagedBookings(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25, CancellationToken cancellationToken = default)
    {
        var totalCount = await _bookingsService.GetCountBookings(cancellationToken);
        var bookings = await _bookingsService.GetPagedBookings(page, limit, cancellationToken);

        var response = bookings
            .Select(b => new BookingsResponse(b.Id, b.StatusId, b.CarId, b.ClientId, b.StartTime, b.EndTime)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("byClient")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<BookingsResponse>>> GetBookingByClientId(CancellationToken cancellationToken)
    {
        var userId = User.GetRequiredUserId();

        var bookings = await _bookingsService.GetBookingsByClient(userId, cancellationToken);
        var response = bookings.Select(b =>
            new BookingsResponse(b.Id, b.StatusId, b.CarId, b.ClientId, b.StartTime, b.EndTime));

        return Ok(response);
    }

    [HttpGet("pagedByClient")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<BookingsResponse>>> GetPagedBookingByClient(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25, CancellationToken cancellationToken = default)
    {
        var userId = User.GetRequiredUserId();

        var totalCount = await _bookingsService.GetCountBookingsByClient(userId, cancellationToken);
        var bookings = await _bookingsService.GetPagedBookingsByClient(userId, page, limit, cancellationToken);

        var response = bookings
            .Select(b => new BookingsResponse(b.Id, b.StatusId, b.CarId, b.ClientId, b.StartTime, b.EndTime)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("byCar/{carId:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<BookingsResponse>>> GetBookingByCarId(int carId, CancellationToken cancellationToken)
    {
        var bookings = await _bookingsService.GetBookingsByCarId(carId, cancellationToken);
        var response = bookings.Select(b =>
            new BookingsResponse(b.Id, b.StatusId, b.CarId, b.ClientId, b.StartTime, b.EndTime));

        return Ok(response);
    }

    [HttpGet("withInfo/{id:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<BookingWithFullInfoDto>>> GetBookingsWithInfo(int id, CancellationToken cancellationToken)
    {
        var response = await _bookingsService.GetBookingWithInfo(id, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<int>> CreateBooking([FromBody] BookingsRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetRequiredUserId();
        var bookingId = await _bookingsService.CreateBooking(
            userId,
            request.StatusId,
            request.CarId,
            request.StartTime,
            request.EndTime, 
            cancellationToken);

        return Ok(bookingId);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> UpdateBooking(int id, [FromBody] BookingsRequest request, CancellationToken cancellationToken)
    {
        var bookingId = await _bookingsService.UpdateBooking(id, request.StatusId, request.CarId, request.ClientId,
            request.StartTime, request.EndTime, cancellationToken);

        return Ok(bookingId);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeleteBooking(int id, CancellationToken cancellationToken)
    {
        return Ok(await _bookingsService.DeleteBooking(id, cancellationToken));
    }
}
