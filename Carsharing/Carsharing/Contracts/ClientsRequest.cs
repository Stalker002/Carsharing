namespace Carsharing.Contracts;

public record ClientsRequest(
    int UserId,
    string Name,
    string Surname,
    string PhoneNumber,
    string Email);