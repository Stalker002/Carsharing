namespace Shared.Contracts.Bookings;

public record BookingsRequest(
    int StatusId,
    int CarId,
    int ClientId,
    DateTime StartTime,
    DateTime EndTime
    );