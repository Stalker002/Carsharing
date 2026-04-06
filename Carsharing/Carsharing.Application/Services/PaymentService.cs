using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<List<Payment>> GetPayments(CancellationToken cancellationToken)
    {
        return await _paymentRepository.Get(cancellationToken);
    }

    public async Task<List<Payment>> GetPagedPayments(int page, int limit, CancellationToken cancellationToken)
    {
        return await _paymentRepository.GetPaged(page, limit, cancellationToken);
    }

    public async Task<int> GetCountPayments(CancellationToken cancellationToken)
    {
        return await _paymentRepository.GetCount(cancellationToken);
    }

    public async Task<List<Payment>> GetPaymentById(int id, CancellationToken cancellationToken)
    {
        return await _paymentRepository.GetById(id, cancellationToken);
    }

    public async Task<List<Payment>> GetPaymentByBillId(int billId, CancellationToken cancellationToken)
    {
        return await _paymentRepository.GetByBillId(billId, cancellationToken);
    }

    public async Task<int> CreatePayment(Payment payment, CancellationToken cancellationToken)
    {
        return await _paymentRepository.Create(payment, cancellationToken);
    }

    public async Task<int> UpdatePayment(int id, int? billId, decimal? sum, string? method, DateTime? date, CancellationToken cancellationToken)
    {
        return await _paymentRepository.Update(id, billId, sum, method, date, cancellationToken);
    }

    public async Task<int> DeletePayment(int id, CancellationToken cancellationToken)
    {
        return await _paymentRepository.Delete(id, cancellationToken);
    }
}
