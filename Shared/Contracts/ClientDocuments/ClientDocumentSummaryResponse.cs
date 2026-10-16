namespace Shared.Contracts.ClientDocuments;

public class ClientDocumentSummaryResponse
{
    public int Id { get; init; }
    public int ClientId { get; init; }
    public string? Type { get; init; }
    public string? LicenseCategory { get; init; }
    public DateOnly IssueDate { get; init; }
    public DateOnly ExpiryDate { get; init; }
}

