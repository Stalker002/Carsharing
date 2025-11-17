using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly CarsharingDbContext _context;

    public BookingRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Booking>> Get()
    {
        var bookingEntity = await _context.Booking
            .AsNoTracking()
            .ToListAsync();

        var bookings = bookingEntity
            .Select(b => Booking.Create(
                b.Id,
                b.StatusId,
                b.CarId,
                b.ClientId,
                b.StartTime,
                b.EndTime).booking)
            .ToList();

        return bookings;
    }

    public async Task<List<Booking>> GetByClientId(int clientId)
    {
        var bookingEntity = await _context.Booking
            .Where(b => b.ClientId == clientId)
            .AsNoTracking()
            .ToListAsync();

        var bookings = bookingEntity
            .Select(b => Booking.Create(
                b.Id,
                b.StatusId,
                b.CarId,
                b.ClientId,
                b.StartTime,
                b.EndTime).booking)
            .ToList();

        return bookings;
    }

    public async Task<int> Create(Booking booking)
    {
        var (_, error) = Booking.Create(
            0,
            booking.StatusId,
            booking.CarId,
            booking.ClientId,
            booking.StartTime,
            booking.EndTime);

        if (!string.IsNullOrEmpty(error))
            throw new ArgumentException($"Create exception booking: {error}");

        var bookingEntities = new BookingEntity
        {
            StatusId = booking.StatusId,
            CarId = booking.CarId,
            ClientId = booking.ClientId,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime
        };

        await _context.Booking.AddAsync(bookingEntities);
        await _context.SaveChangesAsync();

        return bookingEntities.Id;
    }

    public async Task<int> Update(int id, int? statusId, int? carId, int? clientId,
        DateTime? startTime, DateTime? endTime)
    {
        var booking = await _context.Booking.FirstOrDefaultAsync(b => b.Id == id)
                      ?? throw new Exception("Booking not found");

        if (statusId.HasValue)
            booking.StatusId = statusId.Value;

        if (carId.HasValue)
            booking.CarId = carId.Value;

        if (clientId.HasValue)
            booking.ClientId = clientId.Value;

        if (startTime.HasValue)
            booking.StartTime = startTime.Value;

        if (endTime.HasValue)
            booking.EndTime = endTime.Value;

        await _context.SaveChangesAsync();

        return booking.Id;
    }

    public async Task<int> Delete(int id)
    {
        var bookingEntity = await _context.Booking
            .Where(b => b.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}