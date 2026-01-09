using Carsharing.Application.Abstractions;
using Carsharing.Application.DTOs;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.Application.Services;

public class BillsService : IBillsService
{
    private readonly IBillRepository _billRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly ITripRepository _tripRepository;
    private readonly IBillStatusRepository _statusRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IPromocodeRepository _promocodeRepository;
    private readonly CarsharingDbContext _context;

    public BillsService(IBillRepository billRepository, IBookingRepository bookingRepository,
        ITripRepository tripRepository, IBillStatusRepository statusRepository, IClientRepository clientRepository,
        IPromocodeRepository promocodeRepository, CarsharingDbContext context)
    {
        _context = context;
        _promocodeRepository = promocodeRepository;
        _clientRepository = clientRepository;
        _statusRepository = statusRepository;
        _tripRepository = tripRepository;
        _bookingRepository = bookingRepository;
        _billRepository = billRepository;
    }

    public async Task<List<Bill>> GetBills()
    {
        return await _billRepository.Get();
    }

    public async Task<List<Bill>> GetPagedBills(int page, int limit)
    {
        return await _billRepository.GetPaged(page, limit);
    }

    public async Task<int> GetBillCount()
    {
        return await _billRepository.GetCount();
    }

    public async Task<List<Bill>> GetBillById(int id)
    {
        return await _billRepository.GetById(id);
    }

    public async Task<List<BillWithMinInfoDto>> GetPagedBillWithMinInfoByUserId(int userId, int page, int limit)
    {
        var client = await _clientRepository.GetClientByUserId(userId);
        var clientId = client.Select(c => c.Id).FirstOrDefault();

        var bookings = await _bookingRepository.GetPagedByClientId(clientId, page, limit);
        var bookingId = bookings.Select(c => c.Id).ToList();

        var trip = await _tripRepository.GetByBookingId(bookingId);
        var tripId = trip.Select(tr => tr.Id).ToList();

        var bills = await _billRepository.GetByTripId(tripId);

        var statuses = await _statusRepository.Get();

        var response = (from b in bills
                        join tr in trip on b.TripId equals tr.Id
                        join s in statuses on b.StatusId equals s.Id
                        select new BillWithMinInfoDto(
                            b.Id,
                            s.Name,
                            b.IssueDate,
                            b.Amount,
                            b.RemainingAmount,
                            tr.TariffType
                        )).ToList();

        return response;
    }

    public async Task<int> GetCountPagedBillWithMinInfoByUser(int userId)
    {
        var client = await _clientRepository.GetClientByUserId(userId);
        var clientId = client.Select(c => c.Id).FirstOrDefault();
        return await _bookingRepository.GetCountByClient(clientId);
    }

    public async Task<List<BillWithInfoDto>> GetBillWithInfoById(int id)
    {
        var bills = await _billRepository.GetById(id);
        var tripId = bills.Select(b => b.TripId).FirstOrDefault();

        var trip = await _tripRepository.GetById(tripId);
        var bookingId = trip.Select(tr => tr.BookingId).FirstOrDefault();

        var bookings = await _bookingRepository.GetById(bookingId);

        var promocodeId = bills
                    .Where(b => b.PromocodeId.HasValue)
                    .Select(b => b.PromocodeId).FirstOrDefault();
        var promocode = await _promocodeRepository.GetById(promocodeId);

        
        var statuses = await _statusRepository.Get();

        var response = (from b in bills
                        join tr in trip on b.TripId equals tr.Id
                        join bo in bookings on tr.BookingId equals bo.Id
                        join s in statuses on b.StatusId equals s.Id
                        join promo in promocode on b.PromocodeId equals promo.Id into promoJoin
                        from promoLeft in promoJoin.DefaultIfEmpty()
                        select new BillWithInfoDto(
                            b.Id,
                            s.Name,
                            promoLeft?.Code,
                            b.IssueDate,
                            b.Amount,
                            b.RemainingAmount,
                            bo.CarId,
                            tr.Duration,
                            tr.Distance,
                            tr.TariffType
                            )).ToList();

        return response;
    }

    public async Task<int> CreateBill(Bill bill)
    {
        return await _billRepository.Create(bill);
    }

    public async Task ApplyPromocode(int billId, string code)
    {
        var promo = await _context.Promocode
            .FirstOrDefaultAsync(p => p.Code == code &&
                                      p.StartDate <= DateOnly.FromDateTime(DateTime.UtcNow) &&
                                      p.EndDate >= DateOnly.FromDateTime(DateTime.UtcNow));

        if (promo == null) throw new Exception("Промокод не найден или истек");

        var bill = await _context.Bill.FirstOrDefaultAsync(b => b.Id == billId);
        if (bill == null) throw new Exception("Счет не найден");

        bill.PromocodeId = promo.Id;

        await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateBill(int id, int? tripId, int? promocodeId, int? statusId, DateTime? issueDate,
        decimal? amount, decimal? remainingAmount)
    {
        return await _billRepository.Update(id, tripId, promocodeId, statusId, issueDate, amount, remainingAmount);
    }

    public async Task<int> DeleteBill(int id)
    {
        return await _billRepository.Delete(id);
    }
}