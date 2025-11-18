namespace Carsharing.Contracts;

public record PaymentRequest(
    int BillId,
    decimal Sum,
    string Method,
    DateTime Date);