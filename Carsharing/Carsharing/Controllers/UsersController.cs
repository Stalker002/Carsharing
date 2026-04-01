using Carsharing.Application.Abstractions;
using Carsharing.Contracts;
using Carsharing.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var (token, error) = await _usersService.Login(request.Login, request.Password, cancellationToken);

        if (!string.IsNullOrEmpty(error)) return BadRequest(new { message = "Неверный логин или пароль" });

        Response.Cookies.Append("tasty", token ?? throw new InvalidOperationException());
        return Ok(new { token });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("tasty");

        return Ok(new { Message = "Logged out" });
    }

    [HttpGet("unpaged")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<UsersResponse>>> GetUsers(CancellationToken cancellationToken)
    {
        var users = await _usersService.GetUsers(cancellationToken);
        var response = users.Select(u => new UsersResponse(u.Id, u.RoleId, u.Login));
        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<UsersResponse>>> GetPagedUsers(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25, CancellationToken cancellationToken = default)
    {
        var totalCount = await _usersService.GetUsersCount(cancellationToken);
        var users = await _usersService.GetPagedUsers(page, limit, cancellationToken);

        var response = users
            .Select(u => new UsersResponse(u.Id, u.RoleId, u.Login)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<UsersResponse>>> GetUserById(int id, CancellationToken cancellationToken = default)
    {
        var users = await _usersService.GetUserById(id, cancellationToken);
        var response = users.Select(u => new UsersResponse(u.Id, u.RoleId, u.Login));
        return Ok(response);
    }

    [HttpGet("MyUser")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<UsersResponse>>> GetMyUser(CancellationToken cancellationToken = default)
    {
        var userId = User.GetRequiredUserId();

        var users = await _usersService.GetUserById(userId, cancellationToken);
        var response = users.Select(u => new UsersResponse(u.Id, u.RoleId, u.Login));
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> CreateUser([FromBody] UsersRequest request, CancellationToken cancellationToken = default)
    {
        var (user, error) = Core.Models.User.Create(
            0,
            request.RoleId,
            request.Login,
            request.Password);

        if (!string.IsNullOrEmpty(error)) return BadRequest(error);

        var userId = await _usersService.CreateUser(user, cancellationToken);

        return Ok(userId);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> UpdateUser(int id, [FromBody] UsersRequest request, CancellationToken cancellationToken)
    {
        var userId = await _usersService.UpdateUser(id, request.RoleId, request.Login, request.Password, cancellationToken);
        return Ok(userId);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeleteUser(int id, CancellationToken cancellationToken)
    {
        return Ok(await _usersService.DeleteUser(id, cancellationToken));
    }
}
