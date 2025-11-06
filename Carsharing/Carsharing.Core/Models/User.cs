namespace Carsharing.Core.Models;

public class User
{
    private const int MaxLoginLength = 100;

    private User(int id,int roleId, string login, string passwordHash)
    {
        Id = id;
        RoleId = roleId;
        Login = login;
        PasswordHash = passwordHash;
    }

    public int Id { get; }

    public int RoleId { get; }

    public string Login { get;}

    public string PasswordHash { get; }
    
    public static (User user, string error) Create(int id, int roleId, string login, string passwordHash)
    {
        var error = string.Empty;

        if (string.IsNullOrEmpty(login) || login.Length > MaxLoginLength)
        {
            error = "Login non correct";
        }

        var user = new User(id, roleId, login, passwordHash);

        return (user, error);
    }
}