namespace Carsharing.DataAccess.Entites;

public class ClientDocumentEntity
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string Type { get; set; }
    public string? LicenseCategory { get; set; }
    public string Number { get; set; }
    public DateOnly IssueDate { get; set; }
    public DateOnly ExpiryDate { get; set; }
    public string FilePath { get; set; }
    public ClientEntity? Client { get; set; }
}