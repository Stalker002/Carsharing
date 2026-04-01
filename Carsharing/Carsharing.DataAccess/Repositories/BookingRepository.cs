using Carsharing.Application.DTOs;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Enum;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class BookingRepository(CarsharingDbContext context) : IBookingRepository
{
    public async Task<List<Booking>> Get(CancellationToken cancellationToken)
    {
        var bookingEntity = await context.Booking
            .AsNoTracking()
            .ToListAsync(cancellationToken);

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

    public async Task<List<Booking>> GetPaged(int page, int limit, CancellationToken cancellationToken)
    {
        var bookingEntity = await context.Booking
            .AsNoTracking()
            .Skip((page - 1) * limit)
            .Take(limit)
            .OrderByDescending(b => b.StartTime)
            .ThenByDescending(b => b.Id)
            .ToListAsync(cancellationToken);

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

    public async Task<int> GetCount(CancellationToken cancellationToken)
    {
        return await context.Booking.CountAsync(cancellationToken);
    }

    public async Task<List<Booking>> GetById(int id, CancellationToken cancellationToken)
    {
        var bookingEntity = await context.Booking
            .Where(b => b.Id == id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

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

    public async Task<List<Booking>> GetByClientId(int clientId, CancellationToken cancellationToken)
    {
        var bookingEntity = await context.Booking
            .Where(b => b.ClientId == clientId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

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

    public async Task<List<Booking>> GetPagedByClientId(int clientId, int page, int limit, CancellationToken cancellationToken)
    {
        var bookingEntity = await context.Booking
            .Where(b => b.ClientId == clientId)
            .OrderByDescending(b => b.StartTime)
            .ThenByDescending(b => b.Id)
            .Skip((page - 1) * limit)
            .Take(limit)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

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

    public async Task<int> GetCountByClient(int clientId, CancellationToken cancellationToken)
    {
        return await context.Booking.Where(b => b.ClientId == clientId).CountAsync(cancellationToken);
    }

    public async Task<List<Booking>> GetByCarId(int carId, CancellationToken cancellationToken)
    {
        var bookingEntity = await context.Booking
            .Where(b => b.CarId == carId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

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

    public async Task<List<BookingWithFullInfoDto>> GetBookingWithInfo(int id, CancellationToken cancellationToken)
    {
        return await context.Booking
            .AsNoTracking()
            .Where(b => b.Id == id)
            .Select(b => new BookingWithFullInfoDto(
                b.Id,
                b.BookingStatus!.Name!,
                $"{b.Client!.Name} {b.Client.Surname}",
                $"{b.Car!.SpecificationCar!.Brand} {b.Car.SpecificationCar.Model} ({b.Car.SpecificationCar.StateNumber})",
                b.StartTime,
                b.EndTime
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<int> Create(Booking booking, CancellationToken cancellationToken)
    {
        var hasActiveBooking = await context.Booking
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

        var bookingEntities = new BookingEntity
        {
            StatusId = booking.StatusId,
            CarId = booking.CarId,
            ClientId = booking.ClientId,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime
        };

        await context.Booking.AddAsync(bookingEntities);
        await context.SaveChangesAsync();

        return bookingEntities.Id;
    }

    public async Task<int> Update(int id, int? statusId, int? carId, int? clientId,
        DateTime? startTime, DateTime? endTime, CancellationToken cancellationToken)
    {
        var booking = await context.Booking.FirstOrDefaultAsync(b => b.Id == id)
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

        await context.SaveChangesAsync();

        return booking.Id;
    }

    public async Task<int> Delete(int id, CancellationToken cancellationToken)
    {
        await context.Booking
            .Where(b => b.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return id;
    }
}
