namespace Carsharing.Core.Models;

public class User
{
    public const int MaxLoginLength = 100;
    public const int MaxPasswordLength = 256;
    public const int MinPasswordLength = 6;

    private User(int id, int roleId, string login, string passwordHash)
    {
        Id = id;
        RoleId = roleId;
        Login = login;
        Password = passwordHash;
    }

    public int Id { get; }

    public int RoleId { get; }

    public string Login { get; }

    public string Password { get; }

    public static (User user, string error) Create(int id, int roleId, string login, string password)
    {
        var error = string.Empty;

        if (roleId < 0)
            error = "Role Id must be positive";

        if (string.IsNullOrWhiteSpace(login))
            error = "Login can't be empty";
        if (login.Length > MaxLoginLength)
            error = $"Login can't be longer than {MaxLoginLength} symbols";

        if (string.IsNullOrWhiteSpace(password))
            error = "Password can't be empty";
        error = password.Length switch
        {
            > MaxPasswordLength => $"Password can't be longer than {MaxPasswordLength} symbols",
            < MinPasswordLength => $"Password can't be shoter than {MinPasswordLength} symbols",
            _ => error
        };

        var user = new User(id, roleId, login, password);

        return (user, error);
    }
}