namespace Shared.Contracts.Clients;

public record ClientsResponse(
    int Id,
    int UserId,
    string? Name,
    string? Surname,
    string? PhoneNumber,
    string? Email);