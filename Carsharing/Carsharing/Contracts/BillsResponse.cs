namespace Carsharing.Contracts;

public record BillsResponse(
    int Id,
    int TripId,
    int? PromocodeId,
    int StatusId,
    DateTime IssueDate,
    decimal? Amount,
    decimal? RemainingAmount
    );