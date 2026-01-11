using Carsharing.Application.Abstractions;
using Carsharing.Application.DTOs;
using Carsharing.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class FavoriteController : ControllerBase
{
    private readonly IFavoriteService _favoritesService;

    public FavoriteController(IFavoriteService favoritesService)
    {
        _favoritesService = favoritesService;
    }

    [HttpGet("ids")]
    public async Task<ActionResult<List<int>>> GetFavoriteIds()
    {
        var userId = int.Parse(User.FindFirst("userId")!.Value);
        var ids = await _favoritesService.GetMyFavoriteCarIds(userId);
        return Ok(ids);
    }

    [HttpGet]
    public async Task<ActionResult<List<CarWithMinInfoDto>>> GetFavorites(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25)
    {
        var userId = int.Parse(User.FindFirst("userId")!.Value);
        var cars = await _favoritesService.GetMyFavoriteCarsPaged(userId, page, limit);
        var totalCount = await _favoritesService.GetMyFavoritesCount(userId);

        Response.Headers.Append("x-total-count", totalCount.ToString());
        return Ok(cars);
    }

    [HttpPost]
    public async Task<IActionResult> AddToFavorites([FromBody] int carId)
    {
        var userId = int.Parse(User.FindFirst("userId")!.Value);
        await _favoritesService.AddToFavorites(userId, carId);
        return Ok();
    }

    [HttpDelete("{carId:int}")]
    public async Task<IActionResult> RemoveFromFavorites(int carId)
    {
        var userId = int.Parse(User.FindFirst("userId")!.Value);
        await _favoritesService.RemoveFromFavorites(userId, carId);
        return Ok();
    }
}
