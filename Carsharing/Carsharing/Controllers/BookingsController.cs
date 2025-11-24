using Carsharing.Application.DTOs;
using Carsharing.Application.Services;
using Carsharing.Contracts;
using Carsharing.Core.Models;
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

    [HttpGet]
    public async Task<ActionResult<List<BookingsResponse>>> GetBookings()
    {
        var bookings = await _bookingsService.GetBookings();
        var response = bookings.Select(b =>
            new BookingsResponse(b.Id, b.StatusId, b.CarId, b.ClientId, b.StartTime, b.EndTime));

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<List<BookingsResponse>>> GetBookingById(int id)
    {
        var bookings = await _bookingsService.GetBookingsById(id);
        var response = bookings.Select(b =>
            new BookingsResponse(b.Id, b.StatusId, b.CarId, b.ClientId, b.StartTime, b.EndTime));

        return Ok(response);
    }

    [HttpGet("byClient/{clientId:int}")]
    public async Task<ActionResult<List<BookingsResponse>>> GetBookingByClientId(int clientId)
    {
        var bookings = await _bookingsService.GetBookingsByClientId(clientId);
        var response = bookings.Select(b =>
            new BookingsResponse(b.Id, b.StatusId, b.CarId, b.ClientId, b.StartTime, b.EndTime));

        return Ok(response);
    }

    [HttpGet("byCar/{carId:int}")]
    public async Task<ActionResult<List<BookingsResponse>>> GetBookingByCarId(int carId)
    {
        var bookings = await _bookingsService.GetBookingsByCarId(carId);
        var response = bookings.Select(b =>
            new BookingsResponse(b.Id, b.StatusId, b.CarId, b.ClientId, b.StartTime, b.EndTime));

        return Ok(response);
    }

    [HttpGet("withInfo/{id:int}")]
    public async Task<ActionResult<List<BookingWithFullInfoDto>>> GetBookingsWithInfo(int id)
    {
        var response = await _bookingsService.GetBookingsWithInfo(id);
        return Ok(response);
    }

    [HttpPost]
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
    public async Task<ActionResult<int>> UpdateBooking(int id, [FromBody] BookingsRequest request)
    {
        var bookingId = await _bookingsService.UpdateBooking(id, request.StatusId, request.CarId, request.ClientId,
            request.StartTime, request.EndTime);

        return Ok(bookingId);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<int>> DeleteBooking(int id)
    {
        return Ok(await _bookingsService.DeleteBooking(id));
    }
}