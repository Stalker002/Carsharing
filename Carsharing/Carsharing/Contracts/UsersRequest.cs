namespace Carsharing.Contracts;

public record UsersRequest(
    int RoleId,
    string Login,
    string Password
);