using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IPaymentService
{
    Task<List<Payment>> GetPayments();
    Task<List<Payment>> GetPagedPayments(int page, int limit);
    Task<int> GetCountPayments();
    Task<List<Payment>> GetPaymentById(int id);
    Task<List<Payment>> GetPaymentByBillId(int billId);
    Task<int> CreatePayment(Payment payment);
    Task<int> UpdatePayment(int id, int? billId, decimal? sum, string? method, DateTime? date);
    Task<int> DeletePayment(int id);
}