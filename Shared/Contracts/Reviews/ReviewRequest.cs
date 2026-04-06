namespace Shared.Contracts.Reviews;

public record ReviewRequest(
    int ClientId,
    int CarId,
    short Rating,
    string Comment,
    DateTime Date);