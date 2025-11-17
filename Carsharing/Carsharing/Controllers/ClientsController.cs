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

    public ClientsController(IClientsService clientsService)
    {
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

    [HttpPost]
    public async Task<ActionResult<int>> CreateClient([FromBody] ClientsRequest request)
    {
        var (client, error) = Client.Create(
            0,
            request.UserId,
            request.Name,
            request.Surname,
            request.PhoneNumber,
            request.Email);

        if (!string.IsNullOrWhiteSpace(error)) return BadRequest(error);

        var clientId = await _clientsService.CreateClient(client);

        return Ok(clientId);
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