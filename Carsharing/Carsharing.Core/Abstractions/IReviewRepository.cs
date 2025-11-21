using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IReviewRepository
{
    Task<List<Review>> Get();

    Task<int> GetCount();

    Task<List<Review>> GetByCarId(int carId);
    Task<int> Create(Review review);

    Task<int> Update(int id, int? clientId, int? carId, short? rating, string? comment,
        DateTime? date);

    Task<int> Delete(int id);
}