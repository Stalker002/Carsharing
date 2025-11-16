using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IBillsService
{
    Task<List<Bill>> GetBills();
    Task<int> CreateBill(Bill bill);

    Task<int> UpdateBill(int id, int? tripId, int? promocodeId, int? statusId, DateTime? issueDate,
        decimal? amount, decimal? remainingAmount);

    Task<int> DeleteBill(int id);
}