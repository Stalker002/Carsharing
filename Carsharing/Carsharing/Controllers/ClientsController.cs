using Carsharing.Application.Abstractions;
using Carsharing.Core.Models;
using Carsharing.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Clients;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientsController(IClientsService clientsService) : ControllerBase
{
    [HttpGet("unpaged")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<ClientsResponse>>> GetClients(CancellationToken cancellationToken)
    {
        var clients = await clientsService.GetClients(cancellationToken);
        var response = clients.Select(cl =>
            new ClientsResponse(cl.Id, cl.UserId, cl.Name, cl.Surname, cl.PhoneNumber, cl.Email));
        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<ClientsResponse>>> GetPagedClients(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25, CancellationToken cancellationToken = default)
    {
        var totalCount = await clientsService.GetCountClients(cancellationToken);
        var clients = await clientsService.GetPagedClients(page, limit, cancellationToken);

        var response = clients
            .Select(cl => new ClientsResponse(cl.Id, cl.UserId, cl.Name, cl.Surname, cl.PhoneNumber, cl.Email))
            .ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<ClientsResponse>>> GetClientById(int id, CancellationToken cancellationToken)
    {
        var clients = await clientsService.GetClientById(id, cancellationToken);
        var response = clients.Select(cl =>
            new ClientsResponse(cl.Id, cl.UserId, cl.Name, cl.Surname, cl.PhoneNumber, cl.Email));

        return Ok(response);
    }

    [HttpGet("ByUserId/{userId:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<ClientsResponse>>> GetClientByUserId(int userId, CancellationToken cancellationToken)
    {
        var clients = await clientsService.GetClientByUserId(userId, cancellationToken);
        var response = clients.Select(cl =>
            new ClientsResponse(cl.Id, cl.UserId, cl.Name, cl.Surname, cl.PhoneNumber, cl.Email));
        return Ok(response);
    }

    [HttpGet("My")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<ClientsResponse>>> GetClientByUserId(CancellationToken cancellationToken)
    {
        var userId = User.GetRequiredUserId();
        var clients = await clientsService.GetClientByUserId(userId, cancellationToken);
        var response = clients.Select(cl =>
            new ClientsResponse(cl.Id, cl.UserId, cl.Name, cl.Surname, cl.PhoneNumber, cl.Email));
        return Ok(response);
    }

    [HttpGet("MyDocuments")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<IActionResult> GetMyDocuments(CancellationToken cancellationToken)
    {
        var userId = User.GetRequiredUserId();

        var clients = await clientsService.GetMyDocuments(userId, cancellationToken);
        var response = clients.Select(d =>
            new
            {
                d.Id,
                d.ClientId,
                d.Type,
                d.LicenseCategory,
                d.IssueDate,
                d.ExpiryDate
            });
        return Ok(response);
    }

    [HttpGet("Documents/{clientId:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetClientDocuments(int clientId, CancellationToken cancellationToken)
    {
        var clients = await clientsService.GetClientDocuments(clientId, cancellationToken);
        var response = clients.Select(d =>
            new
            {
                d.Id,
                d.ClientId,
                d.Type,
                d.LicenseCategory,
                d.IssueDate,
                d.ExpiryDate
            });
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateClient(ClientsRequest request, CancellationToken cancellationToken)
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

        var clientId = await clientsService.CreateClient(client, cancellationToken);

        return Ok(clientId);
    }

    [HttpPost("with-user")]
    public async Task<ActionResult<int>> CreateClientWithUser([FromBody] ClientRegistrationRequest request, CancellationToken cancellationToken)
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

        var clientId = await clientsService.CreateClientWithUser(client, user, cancellationToken);

        return Ok(new { ClientId = clientId, Message = "Registration successful" });
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<int>> UpdateClient(int id, [FromBody] ClientsRequest request, CancellationToken cancellationToken)
    {
        int clientId;

        if (User.IsAdmin())
        {
            clientId = await clientsService.UpdateClient(
                id,
                request.UserId,
                request.Name,
                request.Surname,
                request.PhoneNumber,
                request.Email,
                cancellationToken);
        }
        else
        {
            var currentUserId = User.GetRequiredUserId();
            var myClient = (await clientsService.GetClientByUserId(currentUserId, cancellationToken)).SingleOrDefault();

            if (myClient == null)
                return NotFound(new { message = "Клиент не найден" });

            if (myClient.Id != id)
                return Forbid();

            clientId = await clientsService.UpdateOwnClient(
                currentUserId,
                request.Name,
                request.Surname,
                request.PhoneNumber,
                request.Email,
                cancellationToken);
        }

        return Ok(clientId);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeleteClient(int id, CancellationToken cancellationToken)
    {
        return Ok(await clientsService.DeleteClient(id, cancellationToken));
    }
}
