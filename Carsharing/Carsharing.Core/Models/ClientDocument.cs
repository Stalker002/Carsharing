namespace Carsharing.Core.Models;

public class ClientDocument
{
    private const int MaxTypeLength = 50;
    private const int MaxNumberLength = 50;
    private const int MaxFilePathLength = 255;

    private ClientDocument(int id, int clientId, string type, string number, DateOnly issueDate, DateOnly expiryDate, string filePath)
    {
        Id = id;
        ClientId = clientId;
        Type = type;
        Number = number;
        IssueDate = issueDate;
        ExpiryDate = expiryDate;
        FilePath = filePath;
    }

    public int Id { get; set; }

    public int ClientId { get; set; }

    public string Type { get; set; } = string.Empty;

    public string Number { get; set; } = string.Empty;

    public DateOnly IssueDate { get; set; }

    public DateOnly ExpiryDate { get; set; }

    public string FilePath { get; set; } = string.Empty;

    public static (ClientDocument clientDocument, string error) Create(int id, int clientId, string type, string number,
        DateOnly issueDate, DateOnly expiryDate, string filePath)
    {
        var error = string.Empty;

        if (clientId < 0)
            error = "Client Id must be positive";

        if (string.IsNullOrWhiteSpace(type))
            error = "Type can't be empty";
        if (type.Length > MaxTypeLength)
            error = $"Type can't be longer than {MaxTypeLength} symbols";

        if (string.IsNullOrWhiteSpace(number))
            error = "Number can't be empty";
        if (number.Length > MaxNumberLength)
            error = $"Type can't be longer than {MaxNumberLength} symbols";

        if (issueDate > DateOnly.FromDateTime(DateTime.Now))
            error = "The date of adding the document cannot be in the future";

        if (expiryDate < DateOnly.FromDateTime(DateTime.Now))
            error = "The expiration date of the document cannot be in the past";

        if (string.IsNullOrWhiteSpace(filePath))
            error = "File path can't be empty";
        if (filePath.Length  > MaxFilePathLength)
            error = $"Type can't be longer than {MaxFilePathLength} symbols";

        var clientDocument = new ClientDocument(id, clientId, type, number, issueDate, expiryDate, filePath);

        return (clientDocument, error);
    }
}