using System.ComponentModel.DataAnnotations.Schema;

namespace Carsharing.DataAccess.Entites;
[Table("users")]
public class UserEntity
{
    [Column("user_id")]
    public int Id { get; set; }
    [Column("user_role_id")]
    public int RoleId { get; set; }
    [Column("user_login")]
    public required string Login { get; set; }
    [Column("user_password_hash")]
    public required string PasswordHash { get; set; }

}