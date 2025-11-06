namespace Carsharing.Contracts;

public record UsersResponce(
    int Id,
    int RoleId,
    string Login,
    string PasswordHash
    );

