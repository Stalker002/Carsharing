using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface IPaymentService
{
    Task<List<Payment>> GetPayments(CancellationToken cancellationToken);

    Task<List<Payment>> GetPagedPayments(int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCountPayments(CancellationToken cancellationToken);

    Task<List<Payment>> GetPaymentById(int id, CancellationToken cancellationToken);

    Task<List<Payment>> GetPaymentByBillId(int billId, CancellationToken cancellationToken);

    Task<List<Payment>> GetPaymentByBillId(int userId, int billId, CancellationToken cancellationToken);

    Task<int> CreatePayment(Payment payment, CancellationToken cancellationToken);

    Task<int> CreatePayment(int userId, Payment payment, CancellationToken cancellationToken);

    Task<int> UpdatePayment(int id, int? billId, decimal? sum, string? method, DateTime? date,
        CancellationToken cancellationToken);

    Task<int> DeletePayment(int id, CancellationToken cancellationToken);
}