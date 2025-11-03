namespace Carsharing.DataAccess.Entites
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public int RoleId { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
    }
}
