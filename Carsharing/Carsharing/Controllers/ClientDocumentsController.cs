using Carsharing.Application.Abstractions;
using Carsharing.Application.DTOs;
using Carsharing.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.ClientDocuments;

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
    public async Task<IActionResult> GetDocuments(CancellationToken cancellationToken)
    {
        var documents = await _clientDocumentsService.GetClientDocuments(cancellationToken);
        var response = documents.Select(d =>
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
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<IActionResult> CreateClientDocument([FromForm] ClientDocumentsRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetRequiredUserId();
        var (id, error) = await _clientDocumentsService.CreateClientDocumentAsync(userId, request, cancellationToken);

        if (!string.IsNullOrEmpty(error)) return BadRequest(new { message = error });

        return Ok(id);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<IActionResult> UpdateClientDocument(int id, [FromForm] ClientDocumentsRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetRequiredUserId();
        var (isSuccess, error) = await _clientDocumentsService.UpdateClientDocumentAsync(userId, id, request, cancellationToken);

        if (isSuccess) return Ok(new { message = "Документ успешно обновлен" });
        if (error == "Document not found") return NotFound(new { message = "Документ не найден" });

        return BadRequest(new { message = error });
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<IActionResult> DeleteClientDocument(int id, CancellationToken cancellationToken)
    {
        var userId = User.GetRequiredUserId();
        var (isSuccess, error) = await _clientDocumentsService.DeleteClientDocumentAsync(userId, id, cancellationToken);

        if (isSuccess) return Ok(new { message = "Документ удален" });
        if (error == "Document not found") return NotFound(new { message = "Документ не найден" });
        return BadRequest(new { message = error });
    }
}
