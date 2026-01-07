using Carsharing.Core.Abstractions;
using Carsharing.Core.Enum;
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

    public async Task<List<Booking>> GetPaged(int page, int limit)
    {
        var bookingEntity = await _context.Booking
            .AsNoTracking()
            .Skip((page - 1) * limit)
            .Take(limit)
            .OrderByDescending(b => b.StartTime)
            .ThenByDescending(b => b.Id)
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

    public async Task<int> GetCount()
    {
        return await _context.Booking.CountAsync();
    }

    public async Task<List<Booking>> GetById(int id)
    {
        var bookingEntity = await _context.Booking
            .Where(b => b.Id == id)
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

    public async Task<List<Booking>> GetPagedByClientId(int clientId, int page, int limit)
    {
        var bookingEntity = await _context.Booking
            .Where(b => b.ClientId == clientId)
            .OrderByDescending(b => b.StartTime)
            .ThenByDescending(b => b.Id)
            .Skip((page - 1) * limit)
            .Take(limit)
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

    public async Task<int> GetCountByClient(int clientId)
    {
        return await _context.Booking.Where(b => b.ClientId == clientId).CountAsync();
    }

    public async Task<List<Booking>> GetByCarId(int carId)
    {
        var bookingEntity = await _context.Booking
            .Where(b => b.CarId == carId)
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
        var hasActiveBooking = await _context.Booking
            .AnyAsync(b =>
                b.ClientId == booking.ClientId &&
                b.StatusId == (int)BookingStatusEnum.Active);

        if (hasActiveBooking)
            throw new ArgumentException(
                "У вас уже есть активная аренда или бронь. Завершите текущую поездку, прежде чем брать новую машину.");

        var (_, error) = Booking.Create(
            0,
            booking.StatusId,
            booking.CarId,
            booking.ClientId,
            booking.StartTime,
            booking.EndTime);

        if (!string.IsNullOrEmpty(error))
            throw new ArgumentException($"Create exception booking: {error}");

        var overlapping = await _context.Car.AnyAsync(c =>
            c.StatusId != (int)CarStatusEnum.Available
            && c.Id == booking.CarId
        );
        if (overlapping)
            throw new ArgumentException("Автомобиль недоступен на выбранное время");

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

        var (_, error) = Booking.Create(
            0,
            booking.StatusId,
            booking.CarId,
            booking.ClientId,
            booking.StartTime,
            booking.EndTime);

        if (!string.IsNullOrEmpty(error))
            throw new ArgumentException($"Create exception booking: {error}");

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