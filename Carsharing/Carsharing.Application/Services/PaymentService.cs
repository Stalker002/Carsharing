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

    public async Task<List<Payment>> GetPayments()
    {
        return await _paymentRepository.Get();
    }

    public async Task<int> CreatePayment(Payment payment)
    {
        return await _paymentRepository.Create(payment);
    }

    public async Task<int> UpdatePayment(int id, int? billId, decimal? sum, string? method, DateTime? date)
    {
        return await _paymentRepository.Update(id, billId, sum, method, date);
    }

    public async Task<int> DeletePayment(int id)
    {
        return await _paymentRepository.Delete(id);
    }
}