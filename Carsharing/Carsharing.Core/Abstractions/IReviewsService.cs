using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IReviewsService
{
    Task<List<Review>> GetReviews();
    Task<int> CreateReview(Review review);

    Task<int> UpdateReview(int id, int? clientId, int? carId, short? rating, string? comment,
        DateTime? date);

    Task<int> DeleteReview(int id);
}