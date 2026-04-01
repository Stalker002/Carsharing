using Carsharing.Application.Abstractions;
using Carsharing.Application.DTOs;
using Carsharing.Contracts;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<ClientDocumentsResponse>>> GetDocuments()
    {
        var documents = await _clientDocumentsService.GetClientDocuments();
        var response = documents.Select(d =>
            new ClientDocumentsResponse(d.Id, d.ClientId, d.Type, d.LicenseCategory, d.Number, d.IssueDate,
                d.ExpiryDate, d.FilePath));
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<IActionResult> CreateClientDocument([FromForm] ClientDocumentsRequest request)
    {
        var (id, error) = await _clientDocumentsService.CreateClientDocumentAsync(request);

        if (!string.IsNullOrEmpty(error)) return BadRequest(new { message = error });

        return Ok(id);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<IActionResult> UpdateClientDocument(int id, [FromForm] ClientDocumentsRequest request)
    {
        var (isSuccess, error) = await _clientDocumentsService.UpdateClientDocumentAsync(id, request);

        if (isSuccess) return Ok(new { message = "Документ успешно обновлен" });
        if (error == "Document not found") return NotFound(new { message = "Документ не найден" });

        return BadRequest(new { message = error });
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<IActionResult> DeleteClientDocument(int id)
    {
        var (isSuccess, error) = await _clientDocumentsService.DeleteClientDocumentAsync(id);

        if (isSuccess) return Ok(new { message = "Документ удален" });
        if (error == "Document not found") return NotFound(new { message = "Документ не найден" });
        return BadRequest(new { message = error });
    }
}