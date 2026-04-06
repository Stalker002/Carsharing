namespace Shared.Contracts.Bills;

public record BillWithMinInfoDto(
    int Id,
    string StatusName,
    DateTime IssueDate,
    decimal? Amount,
    decimal? RemainingAmount,
    string? TariffType
);