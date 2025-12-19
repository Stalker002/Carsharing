using Microsoft.AspNetCore.Http;

namespace Carsharing.Application.DTOs;

public record ClientDocumentsRequest(
    int ClientId,
    string Type,
    string? LicenseCategory,
    string Number,
    DateOnly IssueDate,
    DateOnly ExpiryDate,
    IFormFile? File);