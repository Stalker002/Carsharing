using Carsharing.Core.Models;
using Shared.Contracts.Reviews;

namespace Carsharing.Application.Abstractions;

public interface IReviewsService
{
    Task<List<Review>> GetReviews(CancellationToken cancellationToken);

    Task<List<Review>> GetPagedReviews(int page, int limit, CancellationToken cancellationToken);

    Task<int> GetReviewsCount(CancellationToken cancellationToken);

    Task<List<ReviewWithClientInfo>> GetReviewsByCarId(int carId, CancellationToken cancellationToken);

    Task<List<ReviewWithClientInfo>> GetPagedReviewsByCarId(int carId, int page, int limit, CancellationToken cancellationToken);

    Task<int> GetReviewsCountsByCarId(int carId, CancellationToken cancellationToken);

    Task<List<Review>> GetReviewById(int id, CancellationToken cancellationToken);

    Task<int> CreateReview(int userId, int carId, short rating, string comment, DateTime date, CancellationToken cancellationToken);

    Task<int> UpdateReview(int userId, int id, int? carId, short? rating, string? comment,
        DateTime? date, CancellationToken cancellationToken);

    Task<int> DeleteReview(int id, CancellationToken cancellationToken);
}
