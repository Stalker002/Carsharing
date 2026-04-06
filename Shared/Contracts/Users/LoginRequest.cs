namespace Shared.Contracts.Users;

public record LoginRequest(
    string Login,
    string Password
);