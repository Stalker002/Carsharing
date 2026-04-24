using Carsharing.Application.Abstractions;
using Carsharing.Application.DTOs;
using Carsharing.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientDocumentsController(IClientDocumentsService clientDocumentsService) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetDocuments(CancellationToken cancellationToken)
    {
        var documents = await clientDocumentsService.GetClientDocuments(cancellationToken);
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
    public async Task<IActionResult> CreateClientDocument([FromForm] ClientDocumentsRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.GetRequiredUserId();
        var (id, error) = await clientDocumentsService.CreateClientDocumentAsync(userId, request, cancellationToken);

        if (!string.IsNullOrEmpty(error)) return BadRequest(new { message = error });

        return Ok(id);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<IActionResult> UpdateClientDocument(int id, [FromForm] ClientDocumentsRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.GetRequiredUserId();
        var (isSuccess, error) =
            await clientDocumentsService.UpdateClientDocumentAsync(userId, id, request, cancellationToken);

        if (isSuccess) return Ok(new { message = "Документ успешно обновлен" });
        if (error == "Document not found") return NotFound(new { message = "Документ не найден" });

        return BadRequest(new { message = error });
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<IActionResult> DeleteClientDocument(int id, CancellationToken cancellationToken)
    {
        var userId = User.GetRequiredUserId();
        var (isSuccess, error) = await clientDocumentsService.DeleteClientDocumentAsync(userId, id, cancellationToken);

        if (isSuccess) return Ok(new { message = "Документ удален" });
        if (error == "Document not found") return NotFound(new { message = "Документ не найден" });
        return BadRequest(new { message = error });
    }
}