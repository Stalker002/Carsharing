using Carsharing.Application.Abstractions;
using Carsharing.Application.DTOs;
using Carsharing.Contracts;
using Carsharing.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<List<BookingsResponse>>> GetBookings()
    {
        var bookings = await _bookingsService.GetBookings();
        var response = bookings.Select(b =>
            new BookingsResponse(b.Id, b.StatusId, b.CarId, b.ClientId, b.StartTime, b.EndTime));

        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<BookingsResponse>>> GetPagedBookings(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25)
    {
        var totalCount = await _bookingsService.GetCountBookings();
        var bookings = await _bookingsService.GetPagedBookings(page, limit);

        var response = bookings
            .Select(b => new BookingsResponse(b.Id, b.StatusId, b.CarId, b.ClientId, b.StartTime, b.EndTime)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("byClient")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<BookingsResponse>>> GetBookingByClientId()
    {
        var userId = int.Parse(User.FindFirst("userId")!.Value);

        var bookings = await _bookingsService.GetBookingsByClient(userId);
        var response = bookings.Select(b =>
            new BookingsResponse(b.Id, b.StatusId, b.CarId, b.ClientId, b.StartTime, b.EndTime));

        return Ok(response);
    }

    [HttpGet("pagedByClient")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<BookingsResponse>>> GetPagedBookingByClient(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25)
    {
        var userId = int.Parse(User.FindFirst("userId")!.Value);

        var totalCount = await _bookingsService.GetCountBookingsByClient(userId);
        var bookings = await _bookingsService.GetPagedBookingsByClient(userId, page, limit);

        var response = bookings
            .Select(b => new BookingsResponse(b.Id, b.StatusId, b.CarId, b.ClientId, b.StartTime, b.EndTime)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("byCar/{carId:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<BookingsResponse>>> GetBookingByCarId(int carId)
    {
        var bookings = await _bookingsService.GetBookingsByCarId(carId);
        var response = bookings.Select(b =>
            new BookingsResponse(b.Id, b.StatusId, b.CarId, b.ClientId, b.StartTime, b.EndTime));

        return Ok(response);
    }

    [HttpGet("withInfo/{id:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<BookingWithFullInfoDto>>> GetBookingsWithInfo(int id)
    {
        var response = await _bookingsService.GetBookingWithInfo(id);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<int>> CreateBooking([FromBody] BookingsRequest request)
    {
        var (booking, error) = Booking.Create(
            0,
            request.StatusId,
            request.CarId,
            request.ClientId,
            request.StartTime,
            request.EndTime);

        if (!string.IsNullOrWhiteSpace(error)) return BadRequest(error);

        var bookingId = await _bookingsService.CreateBooking(booking);

        return Ok(bookingId);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> UpdateBooking(int id, [FromBody] BookingsRequest request)
    {
        var bookingId = await _bookingsService.UpdateBooking(id, request.StatusId, request.CarId, request.ClientId,
            request.StartTime, request.EndTime);

        return Ok(bookingId);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeleteBooking(int id)
    {
        return Ok(await _bookingsService.DeleteBooking(id));
    }
}