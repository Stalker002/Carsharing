using Carsharing.Application.Services;
using Carsharing.Contracts;
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
    public async Task<ActionResult<List<UsersResponce>>> GetUsers()
    {
        var users = await _usersService.GetUsers();
        var responce = users.Select(u => new UsersResponce(u.Id,u.RoleId,u.Login,u.PasswordHash));
        return Ok(responce);
    }
}
