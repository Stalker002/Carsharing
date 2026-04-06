namespace Shared.Contracts.Payments;

public record PaymentsRequest(
    int BillId,
    decimal Sum,
    string Method,
    DateTime Date);