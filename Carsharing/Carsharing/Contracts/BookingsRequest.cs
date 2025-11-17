namespace Carsharing.Contracts;

public record BookingsRequest(
    int StatusId,
    int CarId,
    int ClientId,
    DateTime StartTime,
    DateTime EndTime
    );