
using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
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

    [HttpGet]
    public async Task<ActionResult<List<MaintenanceResponse>>> GetMaintenances()
    {
        var maintenances = await _reviewsService.GetReviews();
        var response =
            maintenances.Select(r => new ReviewResponse(r.Id, r.ClientId, r.CarId, r.Rating, r.Comment, r.Date));

        return Ok(response);
    }

    [HttpPost]
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
    public async Task<ActionResult<int>> UpdateReview(int id, [FromBody] ReviewRequest request)
    {
        var reviewId = await _reviewsService.UpdateReview(id, request.ClientId, request.CarId, request.Rating,
            request.Comment, request.Date);
        return Ok(reviewId);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<int>> DeleteReview(int id)
    {
        return Ok(await _reviewsService.DeleteReview(id));
    }
}