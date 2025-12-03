namespace Carsharing.Contracts;

public record ClientRegistrationRequest(
    string Name,
    string Surname,
    string PhoneNumber,
    string Email,
    string Login,
    string Password);