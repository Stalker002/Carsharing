namespace Carsharing.Contracts;

public record ReviewResponse(
    int Id,
    int ClientId,
    int CarId,
    short Rating,
    string? Comment,
    DateTime Date);