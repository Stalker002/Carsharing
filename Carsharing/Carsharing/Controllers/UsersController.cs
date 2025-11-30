using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
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
    public async Task<ActionResult<string>> Login([FromBody] LoginRequest loginRequest)
    {
        var token = await _usersService.Login(loginRequest.Login, loginRequest.Password);

        Response.Cookies.Append("tasty", token);

        return Ok(new { Message = "Logged in" });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("tasty");

        return Ok(new { Message = "Logged out" });
    }

    [HttpGet("unpaged")]
    public async Task<ActionResult<List<UsersResponse>>> GetUsers()
    {
        var users = await _usersService.GetUsers();
        var response = users.Select(u => new UsersResponse(u.Id, u.RoleId, u.Login, u.PasswordHash));
        return Ok(response);
    }


    [HttpGet]
    public async Task<ActionResult<List<UsersResponse>>> GetPagedUsers(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25)
    {
        var totalCount = await _usersService.GetUsersCount();
        var users = await _usersService.GetPagedUsers(page, limit);

        var response = users
            .Select(u => new UsersResponse(u.Id, u.RoleId, u.Login, u.PasswordHash)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<List<UsersResponse>>> GetUserById(int id)
    {
        var users = await _usersService.GetUserById(id);
        var response = users.Select(u => new UsersResponse(u.Id, u.RoleId, u.Login, u.PasswordHash));
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateUser([FromBody] UsersRequest request)
    {
        var (user, error) = Core.Models.User.Create(
            0,
            request.RoleId,
            request.Login,
            request.PasswordHash);

        if (!string.IsNullOrEmpty(error)) return BadRequest(error);

        var userId = await _usersService.CreateUser(user);

        return Ok(userId);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<int>> UpdateUser(int id, [FromBody] UsersRequest request)
    {
        var userId = await _usersService.UpdateUser(id, request.RoleId, request.Login, request.PasswordHash);
        return Ok(userId);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<int>> DeleteUser(int id)
    {
        return Ok(await _usersService.DeleteUser(id));
    }
}