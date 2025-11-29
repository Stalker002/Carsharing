using Carsharing.Application.DTOs;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public interface IBillsService
{
    Task<List<Bill>> GetBills();
    Task<List<Bill>> GetPagedBills(int page, int limit);
    Task<int> GetBillCount();
    Task<List<Bill>> GetBillById(int id);
    Task<List<BillWithMinInfoDto>> GetPagedBillWithMinInfoByUserId(int userId, int page, int limit);
    Task<int> GetCountPagedBillWithMinInfoByUser(int userId);
    Task<List<BillWithInfoDto>> GetBillWithInfoById(int id);
    Task<int> CreateBill(Bill bill);
    Task<int> UpdateBill(int id, int? tripId, int? promocodeId, int? statusId, DateTime? issueDate,
        decimal? amount, decimal? remainingAmount);
    Task<int> DeleteBill(int id);
}