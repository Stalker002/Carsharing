namespace Shared.Contracts.Reviews;

public record ReviewWithClientInfo(
    int Id,
    string? Name,
    string? Surname,
    short Rating,
    string? Comment,
    DateTime Date);