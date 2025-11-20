namespace Carsharing.Contracts;

public record ClientRegistrationRequest(
    int RoleId,
    string Name,
    string Surname,
    string PhoneNumber,
    string Email,
    string Login,
    string PasswordHash);