using Carsharing.Application.DTOs;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public interface IReviewsService
{
    Task<List<Review>> GetReviews();
    Task<List<Review>> GetPagedReviews(int page, int limit);
    Task<int> GetReviewsCount();
    Task<List<ReviewWithClientInfo>> GetReviewsByCarId(int carId);
    Task<List<ReviewWithClientInfo>> GetPagedReviewsByCarId(int carId, int page, int limit);
    Task<int> GetReviewsCountsByCarId(int carId);
    Task<List<Review>> GetReviewById(int id);
    Task<int> CreateReview(Review review);

    Task<int> UpdateReview(int id, int? clientId, int? carId, short? rating, string? comment,
        DateTime? date);

    Task<int> DeleteReview(int id);
}