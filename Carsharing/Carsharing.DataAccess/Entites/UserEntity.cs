namespace Carsharing.DataAccess.Entites;

public class UserEntity
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public required string Login { get; set; }

    public required string PasswordHash { get; set; }

    public ClientEntity? Client { get; set; }

    public RoleEntity? Role { get; set; }
}