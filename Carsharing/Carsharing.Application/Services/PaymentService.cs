using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Exceptions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IBillRepository _billRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly ITripRepository _tripRepository;

    public PaymentService(
        IPaymentRepository paymentRepository,
        IBillRepository billRepository,
        ITripRepository tripRepository,
        IBookingRepository bookingRepository,
        IClientRepository clientRepository)
    {
        _billRepository = billRepository;
        _tripRepository = tripRepository;
        _bookingRepository = bookingRepository;
        _clientRepository = clientRepository;
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

    public async Task<List<Payment>> GetPaymentByBillId(int userId, int billId, CancellationToken cancellationToken)
    {
        await EnsureBillBelongsToUser(userId, billId, cancellationToken);
        return await _paymentRepository.GetByBillId(billId, cancellationToken);
    }

    public async Task<int> CreatePayment(Payment payment, CancellationToken cancellationToken)
    {
        return await _paymentRepository.Create(payment, cancellationToken);
    }

    public async Task<int> CreatePayment(int userId, Payment payment, CancellationToken cancellationToken)
    {
        await EnsureBillBelongsToUser(userId, payment.BillId, cancellationToken);
        return await _paymentRepository.Create(payment, cancellationToken);
    }

    public async Task<int> UpdatePayment(int id, int? billId, decimal? sum, string? method, DateTime? date,
        CancellationToken cancellationToken)
    {
        return await _paymentRepository.Update(id, billId, sum, method, date, cancellationToken);
    }

    public async Task<int> DeletePayment(int id, CancellationToken cancellationToken)
    {
        return await _paymentRepository.Delete(id, cancellationToken);
    }

    private async Task EnsureBillBelongsToUser(int userId, int billId, CancellationToken cancellationToken)
    {
        var client = (await _clientRepository.GetClientByUserId(userId, cancellationToken)).FirstOrDefault()
                     ?? throw new NotFoundException("Client not found");

        var bill = await _billRepository.GetById(billId, cancellationToken)
                   ?? throw new NotFoundException("Bill not found");

        var trip = (await _tripRepository.GetById(bill.TripId, cancellationToken)).FirstOrDefault()
                   ?? throw new NotFoundException("Trip not found");

        var booking = (await _bookingRepository.GetById(trip.BookingId, cancellationToken)).FirstOrDefault()
                      ?? throw new NotFoundException("Booking not found");

        if (booking.ClientId != client.Id)
            throw new UnauthorizedAccessException("Bill does not belong to current user");
    }
}