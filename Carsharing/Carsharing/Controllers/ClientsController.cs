using Carsharing.Application.Abstractions;
using Carsharing.Contracts;
using Carsharing.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientsService _clientsService;

    public ClientsController(IClientsService clientsService)
    {
        _clientsService = clientsService;
    }

    [HttpGet("unpaged")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<ClientsResponse>>> GetClients()
    {
        var clients = await _clientsService.GetClients();
        var response = clients.Select(cl =>
            new ClientsResponse(cl.Id, cl.UserId, cl.Name, cl.Surname, cl.PhoneNumber, cl.Email));
        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<ClientsResponse>>> GetPagedClients(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25)
    {
        var totalCount = await _clientsService.GetCountClients();
        var clients = await _clientsService.GetPagedClients(page, limit);

        var response = clients
            .Select(cl => new ClientsResponse(cl.Id, cl.UserId, cl.Name, cl.Surname, cl.PhoneNumber, cl.Email))
            .ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<ClientsResponse>>> GetClientById(int id)
    {
        var clients = await _clientsService.GetClientById(id);
        var response = clients.Select(cl =>
            new ClientsResponse(cl.Id, cl.UserId, cl.Name, cl.Surname, cl.PhoneNumber, cl.Email));

        return Ok(response);
    }

    [HttpGet("ByUserId/{userId:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<ClientsResponse>>> GetClientByUserId(int userId)
    {
        var clients = await _clientsService.GetClientByUserId(userId);
        var response = clients.Select(cl =>
            new ClientsResponse(cl.Id, cl.UserId, cl.Name, cl.Surname, cl.PhoneNumber, cl.Email));
        return Ok(response);
    }

    [HttpGet("My")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<ClientsResponse>>> GetClientByUserId()
    {
        var userId = int.Parse(User.FindFirst("userId")!.Value);
        var clients = await _clientsService.GetClientByUserId(userId);
        var response = clients.Select(cl =>
            new ClientsResponse(cl.Id, cl.UserId, cl.Name, cl.Surname, cl.PhoneNumber, cl.Email));
        return Ok(response);
    }

    [HttpGet("MyDocuments")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<ClientsResponse>>> GetMyDocuments()
    {
        var userId = int.Parse(User.FindFirst("userId")!.Value);

        var clients = await _clientsService.GetMyDocuments(userId);
        var response = clients.Select(d =>
            new ClientDocumentsResponse(d.Id, d.ClientId, d.Type, d.LicenseCategory, d.Number, d.IssueDate,
                d.ExpiryDate, d.FilePath));
        return Ok(response);
    }

    [HttpGet("Documents/{clientId:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<ClientsResponse>>> GetClientDocuments(int clientId)
    {
        var clients = await _clientsService.GetClientDocuments(clientId);
        var response = clients.Select(d =>
            new ClientDocumentsResponse(d.Id, d.ClientId, d.Type, d.LicenseCategory, d.Number, d.IssueDate,
                d.ExpiryDate, d.FilePath));
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateClient(ClientsRequest request)
    {
        var (client, clientError) = Client.Create(
            0,
            0,
            request.Name,
            request.Surname,
            request.PhoneNumber,
            request.Email);

        if (clientError is { Length: > 0 })
            return BadRequest(clientError);

        var clientId = await _clientsService.CreateClient(client!);

        return Ok(clientId);
    }

    [HttpPost("with-user")]
    public async Task<ActionResult<int>> CreateClientWithUser([FromBody] ClientRegistrationRequest request)
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

        var clientId = await _clientsService.CreateClientWithUser(client!, user!);

        return Ok(new { ClientId = clientId, Message = "Registration successful"});
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<int>> UpdateClient(int id, [FromBody] ClientsRequest request)
    {
        var clientId =
            await _clientsService.UpdateClient(id, request.UserId, request.Name, request.Surname, request.PhoneNumber,
                request.Email);
        return Ok(clientId);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeleteClient(int id)
    {
        return Ok(await _clientsService.DeleteClient(id));
    }
}