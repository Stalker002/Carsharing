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

    public async Task<List<Review>> GetReviews(CancellationToken cancellationToken)
    {
        return await _reviewRepository.Get(cancellationToken);
    }

    public async Task<List<Review>> GetPagedReviews(int page, int limit, CancellationToken cancellationToken = default)
    {
        return await _reviewRepository.GetPaged(page, limit, cancellationToken);
    }

    public async Task<int> GetReviewsCount(CancellationToken cancellationToken)
    {
        return await _reviewRepository.GetCount(cancellationToken);
    }

    public async Task<List<Review>> GetReviewById(int id, CancellationToken cancellationToken = default)
    {
        return await _reviewRepository.GetById(id, cancellationToken);
    }

    public async Task<List<ReviewWithClientInfo>> GetReviewsByCarId(int carId, CancellationToken cancellationToken = default)
    {
        return await _reviewRepository.GetByCarId(carId, cancellationToken);
    }

    public async Task<List<ReviewWithClientInfo>> GetPagedReviewsByCarId(int carId, int page, int limit, CancellationToken cancellationToken = default)
    {
        return await _reviewRepository.GetPagedByCarId(carId, page, limit, cancellationToken);
    }

    public async Task<int> GetReviewsCountsByCarId(int carId, CancellationToken cancellationToken = default)
    {
        return await _reviewRepository.GetCountByCarId(carId, cancellationToken);
    }

    public async Task<int> CreateReview(int userId, int carId, short rating, string comment, DateTime date, CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetClientByUserId(userId, cancellationToken);
        var clientId = client.Select(c => c.Id).FirstOrDefault();

        if (clientId == 0)
            throw new Exception("Client not found");

        var (review, error) = Review.Create(0, clientId, carId, rating, comment, date);

        if (!string.IsNullOrWhiteSpace(error))
            throw new ArgumentException(error);

        return await _reviewRepository.Create(review, cancellationToken);
    }

    public async Task<int> UpdateReview(int userId, int id, int? carId, short? rating, string? comment,
        DateTime? date, CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetClientByUserId(userId, cancellationToken);
        var clientId = client.Select(c => c.Id).FirstOrDefault();

        if (clientId == 0)
            throw new Exception("Client not found");

        var review = (await _reviewRepository.GetById(id, cancellationToken)).FirstOrDefault()
            ?? throw new Exception("Review not found");

        if (review.ClientId != clientId)
            throw new UnauthorizedAccessException("Review does not belong to current user");

        return await _reviewRepository.Update(id, clientId, carId, rating, comment, date, cancellationToken);
    }

    public async Task<int> DeleteReview(int id, CancellationToken cancellationToken = default)
    {
        return await _reviewRepository.Delete(id, cancellationToken);
    }
}
