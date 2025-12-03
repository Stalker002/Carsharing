using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Carsharing.Controllers;
[ApiController]
[Route("[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientsService _clientsService;
    private readonly IUsersService _usersService;
    private readonly CarsharingDbContext _context;

    public ClientsController(IClientsService clientsService, IUsersService usersService, CarsharingDbContext context)
    {
        _context = context;
        _usersService = usersService;
        _clientsService = clientsService;
    }

    [HttpGet("unpaged")]
    public async Task<ActionResult<List<ClientsResponse>>> GetClients()
    {
        var clients = await _clientsService.GetClients();
        var response = clients.Select(cl =>
            new ClientsResponse(cl.Id, cl.UserId, cl.Name, cl.Surname, cl.PhoneNumber, cl.Email));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<ClientsResponse>>> GetPagedClients(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25)
    {
        var totalCount = await _clientsService.GetCountClients();
        var clients = await _clientsService.GetPagedClients(page, limit);

        var response = clients
            .Select(cl => new ClientsResponse(cl.Id, cl.UserId, cl.Name, cl.Surname, cl.PhoneNumber, cl.Email)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

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

    [HttpGet("MyDocuments")]
    public async Task<ActionResult<List<ClientsResponse>>> GetMyDocuments()
    {
        var userId = int.Parse(User.FindFirst("userId")!.Value);

        var clients = await _clientsService.GetMyDocuments(userId);
        var response = clients.Select(d =>
            new ClientDocumentsResponse(d.Id, d.ClientId, d.Type, d.Number, d.IssueDate, d.ExpiryDate, d.FilePath));
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
        var userExists = await _usersService.GetUserByLogin(request.Login);
        if (userExists != null)
            return Conflict("Пользователь с таким логином уже существует");

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var (user, userError) = Core.Models.User.Create(
                0,
                2,
                request.Login,
                request.Password);

            if (userError is { Length: > 0 })
                return BadRequest(userError);

            var (client, clientError) = Client.Create(
                0,
                0,
                request.Name,
                request.Surname,
                request.PhoneNumber,
                request.Email);

            if (clientError is { Length: > 0 })
                return BadRequest(clientError);

            client.UserId = await _usersService.CreateUser(user);
            var clientId = await _clientsService.CreateClient(client);

            await transaction.CommitAsync();

            return Ok(new
            {
                Message = "Registration successful",
                UserId = client.UserId,
                ClientId = clientId
            });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, $"Server error: {ex.Message}");
        }
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