using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.DataAccess.Entites;

[Table("roles")]
public class RoleEntity
{
    [Column("role_id")]
    public int Id { get; set; }

    [Column("role_name")]
    public string Name { get; set; }

    public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
}