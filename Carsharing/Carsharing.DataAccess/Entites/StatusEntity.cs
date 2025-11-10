using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.DataAccess.Entites;

[Table("status")]
public class StatusEntity
{
    [Column("status_id")]
    public int Id { get; set; }

    [Column("status_name")]
    public string Name { get; set; } = string.Empty;

    [Column("status_description")]
    public string Description { get; set; } = string.Empty;
}