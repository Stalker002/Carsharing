namespace Carsharing.Contracts;

public record PaymentResponse(
    int Id, 
    int BillId, 
    decimal Sum, 
    string Method, 
    DateTime Date);