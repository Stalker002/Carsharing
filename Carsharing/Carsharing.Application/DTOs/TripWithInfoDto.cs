namespace Carsharing.Application.DTOs;

public record TripWithInfoDto(
    int Id,
    int BookingId,
    int StatusId,
    string StartLocation,
    string EndLocation,
    bool InsuranceActive,
    decimal? FuelUsed,
    decimal? Refueled,
    string TariffType,
    DateTime StartTime,
    DateTime? EndTime,
    decimal? Duration,
    decimal? Distance);