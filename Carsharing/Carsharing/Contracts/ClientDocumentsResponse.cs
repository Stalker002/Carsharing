namespace Carsharing.Contracts;

public record ClientDocumentsResponse(
    int Id,
    int ClientId,
    string Type,
    string? LicenseCategory,
    string Number,
    DateOnly IssueDate,
    DateOnly ExpiryDate,
    string FilePath);