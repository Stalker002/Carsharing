namespace Shared.Contracts.Trip;

public record TripUpdateRequest(
    int BookingId,
    int StatusId,
    string TariffType,
    DateTime StartTime,
    DateTime? EndTime,
    decimal? Duration,
    decimal? Distance);