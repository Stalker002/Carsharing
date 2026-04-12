using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly CarsharingDbContext _context;

    public PaymentRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Payment>> Get(CancellationToken cancellationToken)
    {
        var paymentEntities = await _context.Payment
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var payments = paymentEntities
            .Select(p => Payment.Create(
                p.Id,
                p.BillId,
                p.Sum,
                p.Method,
                p.Date).payment)
            .ToList();

        return payments;
    }

    public async Task<List<Payment>> GetPaged(int page, int limit, CancellationToken cancellationToken)
    {
        var paymentEntities = await _context.Payment
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync(cancellationToken);

        var payments = paymentEntities
            .Select(p => Payment.Create(
                p.Id,
                p.BillId,
                p.Sum,
                p.Method,
                p.Date).payment)
            .ToList();

        return payments;
    }

    public async Task<int> GetCount(CancellationToken cancellationToken)
    {
        return await _context.Payment.CountAsync(cancellationToken);
    }

    public async Task<List<Payment>> GetById(int id, CancellationToken cancellationToken)
    {
        var paymentEntities = await _context.Payment
            .Where(p => p.Id == id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var payments = paymentEntities
            .Select(p => Payment.Create(
                p.Id,
                p.BillId,
                p.Sum,
                p.Method,
                p.Date).payment)
            .ToList();

        return payments;
    }

    public async Task<List<Payment>> GetByBillId(int billId, CancellationToken cancellationToken)
    {
        var paymentEntities = await _context.Payment
            .Where(p => p.BillId == billId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var payments = paymentEntities
            .Select(p => Payment.Create(
                p.Id,
                p.BillId,
                p.Sum,
                p.Method,
                p.Date).payment)
            .ToList();

        return payments;
    }

    public async Task<int> Create(Payment payment, CancellationToken cancellationToken)
    {
        var (_, error) = Payment.Create(
            payment.Id,
            payment.BillId,
            payment.Sum,
            payment.Method,
            payment.Date);

        if (!string.IsNullOrWhiteSpace(error))
            throw new Exception($"Payment create error: {error}");

        var paymentEntity = new PaymentEntity
        {
            Id = payment.Id,
            BillId = payment.BillId,
            Sum = payment.Sum,
            Method = payment.Method,
            Date = payment.Date
        };

        await _context.Payment.AddAsync(paymentEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return paymentEntity.Id;
    }

    public async Task<int> Update(int id, int? billId, decimal? sum, string? method, DateTime? date, CancellationToken cancellationToken)
    {
        var payment = await _context.Payment.FirstOrDefaultAsync(p => p.Id == id, cancellationToken: cancellationToken)
                      ?? throw new Exception("Payment not found");

        if (billId.HasValue)
            payment.BillId = billId.Value;

        if (sum.HasValue)
            payment.Sum = sum.Value;

        if (!string.IsNullOrWhiteSpace(method))
            payment.Method = method;

        if (date.HasValue)
            payment.Date = date.Value;

        var (_, error) = Payment.Create(
            payment.Id,
            payment.BillId,
            payment.Sum,
            payment.Method,
            payment.Date);

        if (!string.IsNullOrWhiteSpace(error))
            throw new Exception($"Payment create error: {error}");

        await _context.SaveChangesAsync(cancellationToken);

        return payment.Id;
    }

    public async Task<int> Delete(int id, CancellationToken cancellationToken)
    {
        var paymentEntity = await _context.Payment
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return id;
    }
}
