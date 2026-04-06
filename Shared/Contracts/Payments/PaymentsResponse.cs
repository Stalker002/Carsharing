namespace Shared.Contracts.Payments;

public record PaymentsResponse(
    int Id,
    int BillId,
    decimal Sum,
    string? Method,
    DateTime Date);