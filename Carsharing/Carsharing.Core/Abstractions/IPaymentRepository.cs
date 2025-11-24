using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IPaymentRepository
{
    Task<List<Payment>> Get();
    Task<List<Payment>> GetById(int id);
    Task<List<Payment>> GetByBillId(int billId);
    Task<int> Create(Payment payment);
    Task<int> Update(int id, int? billId, decimal? sum, string? method, DateTime? date);
    Task<int> Delete(int id);
}