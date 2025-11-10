using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.DataAccess.Entites;

[Table("roles")]
public class RoleEntity
{
    [Column("role_id")]
    public int Id { get; }

    [Column("role_name")]
    public string Name { get; }
}