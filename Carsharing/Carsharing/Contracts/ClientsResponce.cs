namespace Carsharing.Contracts;

public record ClientsResponse(
    int Id,
    int UserId,
    string Name,
    string Surname,
    string PhoneNumber,
    string Email);