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

    public async Task<List<BookingStatus>> Get(CancellationToken cancellationToken)
    {
        var bookingStatusEntities = await _context.BookingStatus
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var bookingStatuses = bookingStatusEntities
            .Select(b => BookingStatus.Create(
                b.Id,
                b.Name!).bookingStatus)
            .ToList();

        return bookingStatuses;
    }

    public async Task<bool> Exists(int id, CancellationToken cancellationToken)
    {
        return await _context.BookingStatus
            .AsNoTracking()
            .AnyAsync(b => b.Id == id, cancellationToken);
    }
}
