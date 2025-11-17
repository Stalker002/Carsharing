using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class BillsService : IBillsService
{
    private readonly IBillRepository _billRepository;

    public BillsService(IBillRepository billRepository)
    {
        _billRepository = billRepository;
    }

    public async Task<List<Bill>> GetBills()
    {
        return await _billRepository.Get();
    }

    public async Task<int> CreateBill(Bill bill)
    {
        return await _billRepository.Create(bill);
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