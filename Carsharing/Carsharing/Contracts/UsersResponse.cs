namespace Carsharing.Contracts;

public record UsersResponse(
    int Id,
    int RoleId,
    string Login,
    string Password
);