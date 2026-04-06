using Carsharing.Core.Models;
using Shared.Contracts.Bookings;

namespace Carsharing.Core.Abstractions;

public interface IBookingRepository
{
    Task<List<Booking>> Get(CancellationToken cancellationToken);

    Task<List<Booking>> GetPaged(int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCount(CancellationToken cancellationToken);

    Task<List<Booking>> GetById(int id, CancellationToken cancellationToken);

    Task<List<Booking>> GetByClientId(int clientId, CancellationToken cancellationToken);

    Task<List<Booking>> GetPagedByClientId(int clientId, int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCountByClient(int clientId, CancellationToken cancellationToken);

    Task<List<Booking>> GetByCarId(int carId, CancellationToken cancellationToken);

    Task<List<BookingWithFullInfoDto>> GetBookingWithInfo(int id, CancellationToken cancellationToken);

    Task<int> Create(Booking booking, CancellationToken cancellationToken);

    Task<int> Update(int id, int? statusId, int? carId, int? clientId,
        DateTime? startTime, DateTime? endTime, CancellationToken cancellationToken);

    Task<int> Delete(int id, CancellationToken cancellationToken);
}