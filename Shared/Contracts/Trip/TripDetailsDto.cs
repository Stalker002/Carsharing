namespace Shared.Contracts.Trip;

public record TripDetailsDto(
    TripHeaderDto Header,
    TripSummaryDto Summary,
    IReadOnlyList<TripChargeBreakdownDto> Breakdown,
    IReadOnlyList<TripEventDto> Events,
    IReadOnlyList<TripPaymentDto> Payments);

public record TripHeaderDto(
    int TripId,
    int? BillId,
    string CarTitle,
    string? CarImage,
    string? Transmission,
    string? RegistrationNumber,
    string? TariffType,
    DateTime StartTime,
    DateTime? EndTime,
    string? StartLocation,
    string? EndLocation);

public record TripSummaryDto(
    decimal? Distance,
    decimal? Duration,
    decimal? TotalAmount,
    decimal? RemainingAmount,
    bool InsuranceActive,
    decimal? FuelUsed,
    decimal? Refueled);

public record TripChargeBreakdownDto(
    string Code,
    string Title,
    decimal? Duration,
    decimal? Distance,
    decimal? Amount,
    string? Formula);

public record TripEventDto(
    string Type,
    DateTime? StartTime,
    decimal? Duration,
    decimal? Amount,
    string? Formula);

public record TripPaymentDto(
    int PaymentId,
    string Title,
    DateTime Date,
    decimal Amount,
    string? Method);
