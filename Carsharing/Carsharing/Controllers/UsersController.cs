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

    [HttpGet]
    public async Task<ActionResult<List<UsersResponse>>> GetUsers()
    {
        var users = await _usersService.GetUsers();
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
    public async Task<ActionResult<int>> UpdateUsers(int id, [FromBody] UsersRequest request)
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