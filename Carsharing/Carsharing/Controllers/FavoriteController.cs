using Carsharing.Application.Abstractions;
using Carsharing.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Cars;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = "AdminClientPolicy")]
public class FavoriteController : ControllerBase
{
    private readonly IFavoriteService _favoritesService;

    public FavoriteController(IFavoriteService favoritesService)
    {
        _favoritesService = favoritesService;
    }

    [HttpGet("ids")]
    public async Task<ActionResult<List<int>>> GetFavoriteIds(CancellationToken cancellationToken)
    {
        var userId = User.GetRequiredUserId();
        var ids = await _favoritesService.GetMyFavoriteCarIds(userId, cancellationToken);
        return Ok(ids);
    }

    [HttpGet]
    public async Task<ActionResult<List<CarWithMinInfoDto>>> GetFavorites(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25, CancellationToken cancellationToken = default)
    {
        var userId = User.GetRequiredUserId();
        var cars = await _favoritesService.GetMyFavoriteCarsPaged(userId, page, limit, cancellationToken);
        var totalCount = await _favoritesService.GetMyFavoritesCount(userId, cancellationToken);

        Response.Headers.Append("x-total-count", totalCount.ToString());
        return Ok(cars);
    }

    [HttpPost]
    public async Task<IActionResult> AddToFavorites([FromBody] int carId, CancellationToken cancellationToken)
    {
        var userId = User.GetRequiredUserId();
        await _favoritesService.AddToFavorites(userId, carId, cancellationToken);
        return Ok();
    }

    [HttpDelete("{carId:int}")]
    public async Task<IActionResult> RemoveFromFavorites(int carId, CancellationToken cancellationToken)
    {
        var userId = User.GetRequiredUserId();
        await _favoritesService.RemoveFromFavorites(userId, carId, cancellationToken);
        return Ok();
    }
}
