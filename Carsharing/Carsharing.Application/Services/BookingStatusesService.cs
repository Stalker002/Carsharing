using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class BookingStatusesService : IBookingStatusesService
{
    private readonly IBookingStatusRepository _bookingStatusRepository;

    public BookingStatusesService(
        IBookingStatusRepository bookingStatusRepository)
    {
        _bookingStatusRepository = bookingStatusRepository;
    }

    public async Task<List<BookingStatus>> GetBookingStatuses(CancellationToken cancellationToken)
    {
        var bookingStatuses = await _bookingStatusRepository.Get(cancellationToken);

        return bookingStatuses;
    }
}