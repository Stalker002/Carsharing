namespace Shared.Contracts.Clients;

public record ClientsRequest(
    int UserId,
    string Name,
    string Surname,
    string PhoneNumber,
    string Email);