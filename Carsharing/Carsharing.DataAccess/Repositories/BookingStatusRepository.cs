using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class BookingStatusRepository : IBookingStatusRepository
{
    private readonly CarsharingDbContext _context;

    public BookingStatusRepository(CarsharingDbContext context)
    {
        _context = context;
    }
    public async Task<List<BookingStatus>> Get()
    {
        var bookingStatusEntities = await _context.BookingStatus
            .AsNoTracking()
            .ToListAsync();

        var bookingStatuses = bookingStatusEntities
            .Select(b => BookingStatus.Create(
                b.Id,
                b.Name).bookingStatus)
            .ToList();

        return bookingStatuses;
    }

    public async Task<bool> Exists(int id)
    {
        return await _context.BookingStatus
            .AsNoTracking()
            .AnyAsync(b => b.Id == id);
    }
}