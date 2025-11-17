namespace Carsharing.Contracts;

public record ClientDocumentsRequest(
    int ClientId,
    string Type,
    string Number,
    DateOnly IssueDate,
    DateOnly ExpiryDate,
    string FilePath);