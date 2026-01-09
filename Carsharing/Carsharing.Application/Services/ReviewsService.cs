using Carsharing.Application.Abstractions;
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

    public async Task<List<Review>> GetPagedReviews(int page, int limit)
    {
        return await _reviewRepository.GetPaged(page, limit);
    }

    public async Task<int> GetReviewsCount()
    {
        return await _reviewRepository.GetCount();
    }

    public async Task<List<Review>> GetReviewById(int id)
    {
        return await _reviewRepository.GetById(id);
    }

    public async Task<List<ReviewWithClientInfo>> GetReviewsByCarId(int carId)
    {
        var review = await _reviewRepository.GetByCarId(carId);
        var clientId = review.Select(r => r.ClientId).FirstOrDefault();

        var client = await _clientRepository.GetById(clientId);

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

    public async Task<List<ReviewWithClientInfo>> GetPagedReviewsByCarId(int carId, int page, int limit)
    {
        var review = await _reviewRepository.GetPagedByCarId(carId, page, limit);
        var clientId = review.Select(r => r.ClientId).FirstOrDefault();

        var client = await _clientRepository.GetById(clientId);

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

    public async Task<int> GetReviewsCountsByCarId(int carId)
    {
        return await _reviewRepository.GetCountByCarId(carId);
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