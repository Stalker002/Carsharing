namespace Shared.Contracts.Users;

public record UsersRequest(
    int RoleId,
    string Login,
    string Password
);