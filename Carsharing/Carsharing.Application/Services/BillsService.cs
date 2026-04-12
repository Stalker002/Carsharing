using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Exceptions;
using Carsharing.Core.Models;
using Shared.Contracts.Bills;

namespace Carsharing.Application.Services;

public class BillsService : IBillsService
{
    private readonly IBillRepository _billRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IPromocodeRepository _promocodeRepository;
    private readonly ITripRepository _tripRepository;

    public BillsService(IBillRepository billRepository, IPromocodeRepository promocodeRepository, ITripRepository tripRepository, IBookingRepository bookingRepository, IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
        _bookingRepository = bookingRepository;
        _tripRepository = tripRepository;
        _promocodeRepository = promocodeRepository;
        _billRepository = billRepository;
    }

    public async Task<List<Bill>> GetBills(CancellationToken cancellationToken)
    {
        return await _billRepository.Get(cancellationToken);
    }

    public async Task<List<Bill>> GetPagedBills(int page, int limit, CancellationToken cancellationToken)
    {
        return await _billRepository.GetPaged(page, limit, cancellationToken);
    }

    public async Task<int> GetBillCount(CancellationToken cancellationToken)
    {
        return await _billRepository.GetCount(cancellationToken);
    }

    public async Task<Bill?> GetBillById(int id, CancellationToken cancellationToken)
    {
        return await _billRepository.GetById(id, cancellationToken);
    }

    public async Task<List<BillWithMinInfoDto>> GetPagedBillWithMinInfoByUserId(int userId, int page, int limit, CancellationToken cancellationToken)
    {
        return await _billRepository.GetPagedMinInfoByUserId(userId, page, limit, cancellationToken);
    }

    public async Task<int> GetCountPagedBillWithMinInfoByUser(int userId, CancellationToken cancellationToken)
    {
        return await _billRepository.GetCountByUserId(userId, cancellationToken);
    }

    public async Task<List<BillWithInfoDto>> GetBillWithInfoById(int id, CancellationToken cancellationToken)
    {
        var billDto = await _billRepository.GetInfoById(id, cancellationToken);

        return billDto == null ? [] : [billDto];
    }

    public async Task<List<BillWithInfoDto>> GetBillWithInfoByUserId(int userId, int id, CancellationToken cancellationToken)
    {
        await EnsureBillBelongsToUser(userId, id, cancellationToken);
        return await GetBillWithInfoById(id, cancellationToken);
    }

    public async Task<int> CreateBill(int userId, Bill bill, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetClientByUserId(userId, cancellationToken);
        var clientId = client.Select(c => c.Id).FirstOrDefault();

        if (clientId == 0)
            throw new NotFoundException("Client not found");

        var trip = (await _tripRepository.GetById(bill.TripId, cancellationToken)).FirstOrDefault()
            ?? throw new NotFoundException("Trip not found");

        var booking = (await _bookingRepository.GetById(trip.BookingId, cancellationToken)).FirstOrDefault()
            ?? throw new NotFoundException("Booking not found");

        if (booking.ClientId != clientId)
            throw new UnauthorizedAccessException("Trip does not belong to current user");

        return await _billRepository.Create(bill, cancellationToken);
    }

    public async Task ApplyPromocode(int billId, string code, CancellationToken cancellationToken)
    {
        await ApplyPromocodeInternal(billId, code, cancellationToken);
    }

    public async Task ApplyPromocode(int userId, int billId, string code, CancellationToken cancellationToken)
    {
        await EnsureBillBelongsToUser(userId, billId, cancellationToken);
        await ApplyPromocodeInternal(billId, code, cancellationToken);
    }

    private async Task ApplyPromocodeInternal(int billId, string code, CancellationToken cancellationToken)
    {
        var promos = await _promocodeRepository.GetByCode(code, cancellationToken);
        var promo = promos.FirstOrDefault(p =>
            p.StartDate <= DateOnly.FromDateTime(DateTime.UtcNow) &&
            p.EndDate >= DateOnly.FromDateTime(DateTime.UtcNow));

        if (promo == null) throw new NotFoundException("Промокод не найден или истек");

        var bill = await _billRepository.GetById(billId, cancellationToken);
        if (bill == null) throw new NotFoundException("Счет не найден");

        await _billRepository.Update(
            bill.Id,
            null,
            promo.Id,
            null,
            null,
            null,
            null,
            cancellationToken
        );
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

    public async Task<int> UpdateBill(int id, int? tripId, int? promocodeId, int? statusId, DateTime? issueDate,
        decimal? amount, decimal? remainingAmount, CancellationToken cancellationToken)
    {
        return await _billRepository.Update(id, tripId, promocodeId, statusId, issueDate, amount, remainingAmount, cancellationToken);
    }

    public async Task<int> DeleteBill(int id, CancellationToken cancellationToken)
    {
        return await _billRepository.Delete(id, cancellationToken);
    }
}
