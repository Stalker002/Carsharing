namespace Carsharing.DataAccess.Entites;

public class RoleEntity
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
}