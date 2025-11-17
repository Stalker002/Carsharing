namespace Carsharing.Contracts;

public record BillsRequest(
    int TripId, 
    int? PromocodeId,
    int StatusId,
    DateTime IssueDate,
    decimal Amount,
    decimal RemainingAmount
    );