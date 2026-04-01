using Carsharing.Application.Abstractions;
using Carsharing.Application.DTOs;
using Carsharing.Contracts;
using Carsharing.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<List<ReviewResponse>>> GetReviews()
    {
        var reviews = await _reviewsService.GetReviews();
        var response =
            reviews.Select(r => new ReviewResponse(r.Id, r.ClientId, r.CarId, r.Rating, r.Comment, r.Date));

        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<ReviewResponse>>> GetPagedReviews(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25)
    {
        var totalCount = await _reviewsService.GetReviewsCount();
        var reviews = await _reviewsService.GetPagedReviews(page, limit);

        var response = reviews
            .Select(r => new ReviewResponse(r.Id, r.ClientId, r.CarId, r.Rating, r.Comment, r.Date)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<ReviewResponse>>> GetReviewById(int id)
    {
        var reviews = await _reviewsService.GetReviewById(id);
        var response =
            reviews.Select(r => new ReviewResponse(r.Id, r.ClientId, r.CarId, r.Rating, r.Comment, r.Date));

        return Ok(response);
    }

    [HttpGet("byCar/{carId:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<ReviewWithClientInfo>>> GetReviewsByCar(int carId)
    {
        var reviews = await _reviewsService.GetReviewsByCarId(carId);

        var response =
            reviews.Select(r => new ReviewWithClientInfo(r.Id, r.Name, r.Surname, r.Rating, r.Comment, r.Date));

        return Ok(response);
    }

    [HttpGet("pagedByCar/{carId:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<ReviewWithClientInfo>>> GetPagedReviewsByCarId(
        int carId,
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 5)
    {
        var totalCount = await _reviewsService.GetReviewsCountsByCarId(carId);
        var reviews = await _reviewsService.GetPagedReviewsByCarId(carId, page, limit);

        var response = reviews
            .Select(r => new ReviewWithClientInfo(r.Id, r.Name, r.Surname, r.Rating, r.Comment, r.Date)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<int>> CreateReview([FromBody] ReviewRequest request)
    {
        var (review, error) = Review.Create(
            0,
            request.ClientId,
            request.CarId,
            request.Rating,
            request.Comment,
            request.Date);

        if (!string.IsNullOrWhiteSpace(error)) return BadRequest(error);

        var reviewId = await _reviewsService.CreateReview(review);

        return Ok(reviewId);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<int>> UpdateReview(int id, [FromBody] ReviewRequest request)
    {
        var reviewId = await _reviewsService.UpdateReview(id, request.ClientId, request.CarId, request.Rating,
            request.Comment, request.Date);
        return Ok(reviewId);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeleteReview(int id)
    {
        return Ok(await _reviewsService.DeleteReview(id));
    }
}