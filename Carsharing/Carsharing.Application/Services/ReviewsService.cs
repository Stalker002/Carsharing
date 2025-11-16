using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class ReviewsService : IReviewsService
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewsService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<List<Review>> GetReviews()
    {
        return await _reviewRepository.Get();
    }

    public async Task<int> CreateReview(Review review)
    {
        return await _reviewRepository.Create(review);
    }

    public async Task<int> UpdateReview(int id, int? clientId, int? carId, short? rating, string? comment,
        DateTime? date)
    {
        return await _reviewRepository.Update(id, clientId, carId, rating, comment, date);
    }

    public async Task<int> DeleteReview(int id)
    {
        return await _reviewRepository.Delete(id);
    }
}