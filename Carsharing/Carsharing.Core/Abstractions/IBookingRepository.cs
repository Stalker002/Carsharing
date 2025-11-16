using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IBookingRepository
{
    Task<List<Booking>> Get();
    Task<List<Booking>> GetByClientId(int clientId);
    Task<int> Create(Booking booking);

    Task<int> Update(int id, int? statusId, int? carId, int? clientId,
        DateTime? startTime, DateTime? endTime);

    Task<int> Delete(int id);
}