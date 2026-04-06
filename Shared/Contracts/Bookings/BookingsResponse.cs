namespace Shared.Contracts.Bookings;

public record BookingsResponse(
    int Id,
    int StatusId,
    int CarId,
    int ClientId,
    DateTime? StartTime,
    DateTime? EndTime);