using Carsharing.Application.Abstractions;
using Carsharing.Application.DTOs;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class BillsService : IBillsService
{
    private readonly IBillRepository _billRepository;
    private readonly IPromocodeRepository _promocodeRepository;

    public BillsService(IBillRepository billRepository, IPromocodeRepository promocodeRepository)
    {
        _promocodeRepository = promocodeRepository;
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

    public async Task<Bill?> GetBillById(int id)
    {
        return await _billRepository.GetById(id);
    }

    public async Task<List<BillWithMinInfoDto>> GetPagedBillWithMinInfoByUserId(int userId, int page, int limit)
    {
        return await _billRepository.GetPagedMinInfoByUserId(userId, page, limit);
    }

    public async Task<int> GetCountPagedBillWithMinInfoByUser(int userId)
    {
        return await _billRepository.GetCountByUserId(userId);
    }

    public async Task<List<BillWithInfoDto>> GetBillWithInfoById(int id)
    {
        var billDto = await _billRepository.GetInfoById(id);

        return billDto == null ? [] : [billDto];
    }

    public async Task<int> CreateBill(Bill bill)
    {
        return await _billRepository.Create(bill);
    }

    public async Task ApplyPromocode(int billId, string code)
    {
        var promos = await _promocodeRepository.GetByCode(code);
        var promo = promos.FirstOrDefault(p =>
            p.StartDate <= DateOnly.FromDateTime(DateTime.UtcNow) &&
            p.EndDate >= DateOnly.FromDateTime(DateTime.UtcNow));

        if (promo == null) throw new Exception("Промокод не найден или истек");

        var bill = await _billRepository.GetById(billId);
        if (bill == null) throw new Exception("Счет не найден");

        await _billRepository.Update(
            bill.Id,
            null,
            promo.Id,
            null,
            null,
            null,
            null
        );
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