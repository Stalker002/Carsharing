using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carsharing.Controllers;
[ApiController]
[Route("[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientsService _clientsService;
    private readonly IUsersService _usersService;

    public ClientsController(IClientsService clientsService, IUsersService usersService)
    {
        _usersService = usersService;
        _clientsService = clientsService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ClientsResponse>>> GetClients()
    {
        var clients = await _clientsService.GetClients();
        var response = clients.Select(cl =>
            new ClientsResponse(cl.Id, cl.UserId, cl.Name, cl.Surname, cl.PhoneNumber, cl.Email));
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<List<ClientsResponse>>> GetClientById(int id)
    {
        var clients = await _clientsService.GetClientById(id);
        var response = clients.Select(cl =>
            new ClientsResponse(cl.Id, cl.UserId, cl.Name, cl.Surname, cl.PhoneNumber, cl.Email));
        return Ok(response);
    }

    [HttpGet("My")]
    public async Task<ActionResult<List<ClientsResponse>>> GetClientByUserId()
    {
        var userId = int.Parse(User.FindFirst("userId")!.Value);
        var clients = await _clientsService.GetClientByUserId(userId);
        var response = clients.Select(cl =>
            new ClientsResponse(cl.Id, cl.UserId, cl.Name, cl.Surname, cl.PhoneNumber, cl.Email));
        return Ok(response);
    }

    [HttpGet("Documents/{clientId:int}")]
    public async Task<ActionResult<List<ClientsResponse>>> GetClientDocuments(int clientId)
    {
        var clients = await _clientsService.GetClientDocuments(clientId);
        var response = clients.Select(d =>
            new ClientDocumentsResponse(d.Id, d.ClientId, d.Type, d.Number, d.IssueDate, d.ExpiryDate, d.FilePath));
        return Ok(response);
    }

    [HttpPost("with-user")]
    public async Task<ActionResult<int>> CreateClient([FromBody] ClientRegistrationRequest request)
    {
        var (user, errorUser) = Core.Models.User.Create(
            0,
            2,
            request.Login,
            request.PasswordHash);

        if (!string.IsNullOrEmpty(errorUser)) return BadRequest(errorUser);

        var userId = await _usersService.CreateUser(user);

        var (client, error) = Client.Create(
            0,
            userId,
            request.Name,
            request.Surname,
            request.PhoneNumber,
            request.Email);

        if (!string.IsNullOrWhiteSpace(error))
        {
            await _usersService.DeleteUser(userId);
            return BadRequest(error);
        }

        var clientId = await _clientsService.CreateClient(client);

        return Ok(new
        {
            Message = "Registration successful",
            UserId = userId,
            ClientId = clientId
        });
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<int>> UpdateClient(int id, [FromBody] ClientsRequest request)
    {
        var clientId =
            await _clientsService.UpdateClient(id, request.UserId, request.Name, request.Surname, request.PhoneNumber, request.Email);
        return Ok(clientId);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<int>> DeleteClient(int id)
    {
        return Ok(await _clientsService.DeleteClient(id));
    }
}