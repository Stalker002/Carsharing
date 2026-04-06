namespace Shared.Contracts.Reviews;

public record ReviewCreateRequest(
    int ClientId,
    int TripId,
    short Rating,
    string Comment,
    DateTime Date);