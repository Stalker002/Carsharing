using Carsharing.Application.Abstractions;
using Carsharing.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Reviews;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewsService _reviewsService;

    public ReviewsController(IReviewsService reviewsService)
    {
        _reviewsService = reviewsService;
    }

    [HttpGet("unpaged")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<ReviewResponse>>> GetReviews(CancellationToken cancellationToken)
    {
        var reviews = await _reviewsService.GetReviews(cancellationToken);
        var response =
            reviews.Select(r => new ReviewResponse(r.Id, r.ClientId, r.CarId, r.Rating, r.Comment, r.Date));

        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<ReviewResponse>>> GetPagedReviews(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25, CancellationToken cancellationToken = default)
    {
        var totalCount = await _reviewsService.GetReviewsCount(cancellationToken);
        var reviews = await _reviewsService.GetPagedReviews(page, limit, cancellationToken);

        var response = reviews
            .Select(r => new ReviewResponse(r.Id, r.ClientId, r.CarId, r.Rating, r.Comment, r.Date)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<ReviewResponse>>> GetReviewById(int id, CancellationToken cancellationToken)
    {
        var reviews = await _reviewsService.GetReviewById(id, cancellationToken);
        var response =
            reviews.Select(r => new ReviewResponse(r.Id, r.ClientId, r.CarId, r.Rating, r.Comment, r.Date));

        return Ok(response);
    }

    [HttpGet("byCar/{carId:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<ReviewWithClientInfo>>> GetReviewsByCar(int carId, CancellationToken cancellationToken)
    {
        var reviews = await _reviewsService.GetReviewsByCarId(carId, cancellationToken);

        var response =
            reviews.Select(r => new ReviewWithClientInfo(r.Id, r.Name, r.Surname, r.Rating, r.Comment, r.Date));

        return Ok(response);
    }

    [HttpGet("pagedByCar/{carId:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<ReviewWithClientInfo>>> GetPagedReviewsByCarId(
        int carId,
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 5, CancellationToken cancellationToken = default)
    {
        var totalCount = await _reviewsService.GetReviewsCountsByCarId(carId, cancellationToken);
        var reviews = await _reviewsService.GetPagedReviewsByCarId(carId, page, limit, cancellationToken);

        var response = reviews
            .Select(r => new ReviewWithClientInfo(r.Id, r.Name, r.Surname, r.Rating, r.Comment, r.Date)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<int>> CreateReview([FromBody] ReviewRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetRequiredUserId();
        var reviewId = await _reviewsService.CreateReview(
            userId,
            request.CarId,
            request.Rating,
            request.Comment,
            request.Date,
            cancellationToken);

        return Ok(reviewId);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<int>> UpdateReview(int id, [FromBody] ReviewRequest request, CancellationToken cancellationToken = default)
    {
        var userId = User.GetRequiredUserId();
        var reviewId = await _reviewsService.UpdateReview(
            userId,
            id,
            request.CarId,
            request.Rating,
            request.Comment,
            request.Date,
            cancellationToken);
        return Ok(reviewId);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeleteReview(int id, CancellationToken cancellationToken = default)
    {
        return Ok(await _reviewsService.DeleteReview(id, cancellationToken));
    }
}
