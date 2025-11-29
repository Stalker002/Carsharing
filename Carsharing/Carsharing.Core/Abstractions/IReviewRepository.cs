using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IReviewRepository
{
    Task<List<Review>> Get();
    Task<List<Review>> GetPaged(int page, int limit);
    Task<int> GetCount();
    Task<List<Review>> GetByCarId(int carId);
    Task<List<Review>> GetPagedByCarId(int carId, int page, int limit);
    Task<int> GetCountByCarId(int carId);
    Task<List<Review>> GetById(int id);

    Task<List<Review>> GetByClientId(int clientId);
    Task<int> Create(Review review);

    Task<int> Update(int id, int? clientId, int? carId, short? rating, string? comment,
        DateTime? date);

    Task<int> Delete(int id);
}