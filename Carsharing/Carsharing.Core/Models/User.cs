namespace Carsharing.Core.Models;

public class User
{
    private const int MAX_LOGIN_LENGTH = 100;
    private User(Guid id,int roleId, string login, string passwordHash)
    {
        Id = id;
        RoleId = roleId;
        Login = login;
        PasswordHash = passwordHash;
    }

    public Guid Id { get; }
    public int RoleId { get; }
    public string Login { get;}
    public string PasswordHash { get; }

    public static (User User, string Eror) Create(Guid id, int roleId, string login, string passwordHash)
    {
        var error = string.Empty;

        if (string.IsNullOrEmpty(login) || login.Length > MAX_LOGIN_LENGTH)
        {
            error = "Login non correct";
        }

        var user = new User(id, roleId, login, passwordHash);

        return (user, error);
    }
}