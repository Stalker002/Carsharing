using Carsharing.Application.DTOs;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public interface IBillsService
{
    Task<List<Bill>> GetBills();
    Task<List<Bill>> GetBillById(int id);
    Task<List<BillWithMinInfoDto>> GetBillWithMinInfoByUserId(int userId);
    Task<List<BillWithInfoDto>> GetBillWithInfoById(int id);
    Task<int> CreateBill(Bill bill);

    Task<int> UpdateBill(int id, int? tripId, int? promocodeId, int? statusId, DateTime? issueDate,
        decimal? amount, decimal? remainingAmount);

    Task<int> DeleteBill(int id);
}