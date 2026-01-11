namespace Carsharing.Application.DTOs;

public record BillWithInfoDto(
    int Id,
    string StatusName,
    string? PromocodeName,
    DateTime IssueDate,
    decimal? Amount,
    decimal? RemainingAmount,
    int CarId,
    decimal? Duration,
    decimal? Distance,
    string? TariffType
    );