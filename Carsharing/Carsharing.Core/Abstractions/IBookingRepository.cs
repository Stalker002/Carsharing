using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IBookingRepository
{
    Task<List<Booking>> Get();
    Task<List<Booking>> GetPaged(int page, int limit);
    Task<int> GetCount();
    Task<List<Booking>> GetById(int id);
    Task<List<Booking>> GetByClientId(int clientId);
    Task<List<Booking>> GetPagedByClientId(int clientId, int page, int limit);
    Task<int> GetCountByClient(int clientId);
    Task<List<Booking>> GetByCarId(int carId);
    Task<int> Create(Booking booking);

    Task<int> Update(int id, int? statusId, int? carId, int? clientId,
        DateTime? startTime, DateTime? endTime);

    Task<int> Delete(int id);
}