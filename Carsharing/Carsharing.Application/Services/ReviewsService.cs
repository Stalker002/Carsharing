using Carsharing.Application.DTOs;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class ReviewsService : IReviewsService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IClientRepository _clientRepository;

    public ReviewsService(IReviewRepository reviewRepository, IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
        _reviewRepository = reviewRepository;
    }

    public async Task<List<Review>> GetReviews()
    {
        return await _reviewRepository.Get();
    }

    public async Task<List<ReviewWithClientInfo>> GetReviewsByCarId(int carId)
    {
        var review = await _reviewRepository.GetByCarId(carId);
        var client = await _clientRepository.Get();

        var response = (from r in review
            join c in client on r.ClientId equals c.Id
            select new ReviewWithClientInfo(
                r.Id,
                c.Name,
                c.Surname,
                r.Rating,
                r.Comment,
                r.Date)).ToList();

        return response;
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