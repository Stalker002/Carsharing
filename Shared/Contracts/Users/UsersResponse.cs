namespace Shared.Contracts.Users;

public record UsersResponse(
    int Id,
    int RoleId,
    string Login,
    string Password
);