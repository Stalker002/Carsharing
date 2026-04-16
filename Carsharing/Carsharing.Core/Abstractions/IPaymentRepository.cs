using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IPaymentRepository
{
    Task<List<Payment>> Get(CancellationToken cancellationToken);

    Task<List<Payment>> GetPaged(int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCount(CancellationToken cancellationToken);

    Task<List<Payment>> GetById(int id, CancellationToken cancellationToken);

    Task<List<Payment>> GetByBillId(int billId, CancellationToken cancellationToken);

    Task<int> Create(Payment payment, CancellationToken cancellationToken);

    Task<int> Update(int id, int? billId, decimal? sum, string? method, DateTime? date,
        CancellationToken cancellationToken);

    Task<int> Delete(int id, CancellationToken cancellationToken);
}