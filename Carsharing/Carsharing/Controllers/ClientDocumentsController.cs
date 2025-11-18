using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientDocumentsController : ControllerBase
{
    private readonly IClientDocumentsService _clientDocumentsService;

    public ClientDocumentsController(IClientDocumentsService clientDocumentsService)
    {
        _clientDocumentsService = clientDocumentsService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ClientDocumentsResponse>>> GetDocuments()
    {
        var documents = await _clientDocumentsService.GetClientDocuments();
        var response = documents.Select(d =>
            new ClientDocumentsResponse(d.Id, d.ClientId, d.Type, d.Number, d.IssueDate, d.ExpiryDate, d.FilePath));
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateClientDocument([FromBody] ClientDocumentsRequest request)
    {
        var (document, error) = ClientDocument.Create(
            0,
            request.ClientId,
            request.Type,
            request.Number,
            request.IssueDate,
            request.ExpiryDate,
            request.FilePath);

        if (!string.IsNullOrWhiteSpace(error)) return BadRequest(error);

        var documentId = await _clientDocumentsService.CreateClientDocument(document);

        return Ok(documentId);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<int>> UpdateDocument(int id, [FromBody] ClientDocumentsRequest request)
    {
        var documentId = await _clientDocumentsService.UpdateClientDocument(id, request.ClientId, request.Type,
            request.Number, request.IssueDate, request.ExpiryDate, request.FilePath);

        return Ok(documentId);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<int>> DeleteDocument(int id)
    {
        return Ok(await _clientDocumentsService.DeleteClientDocument(id));
    }
}