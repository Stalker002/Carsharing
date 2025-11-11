using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.DataAccess.Entites;

[Table("client_documents")]
public class ClientDocumentEntity
{
    [Column("document_id")]
    public int Id { get; set; }

    [Column("document_client_id")]
    public int ClientId { get; set; }

    [Column("document_type")]
    public string Type { get; set; }

    [Column("document_number")]
    public string Number { get; set; }

    [Column("document_issue_date")]
    public DateOnly IssueDate { get; set; }

    [Column("document_expiry_date")]
    public DateOnly ExpiryDate { get; set; }

    [Column("document_file_path")]
    public string FilePath { get; set; }

    public UserEntity User { get; set; }
}