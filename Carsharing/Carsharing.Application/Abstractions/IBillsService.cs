using Carsharing.Core.Models;
using Shared.Contracts.Bills;

namespace Carsharing.Application.Abstractions;

public interface IBillsService
{
    Task<List<Bill>> GetBills(CancellationToken cancellationToken);

    Task<List<Bill>> GetPagedBills(int page, int limit, CancellationToken cancellationToken);

    Task<int> GetBillCount(CancellationToken cancellationToken);

    Task<Bill?> GetBillById(int id, CancellationToken cancellationToken);

    Task<List<BillWithMinInfoDto>> GetPagedBillWithMinInfoByUserId(int userId, int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCountPagedBillWithMinInfoByUser(int userId, CancellationToken cancellationToken);

    Task<List<BillWithInfoDto>> GetBillWithInfoById(int id, CancellationToken cancellationToken);

    Task<List<BillWithInfoDto>> GetBillWithInfoByUserId(int userId, int id, CancellationToken cancellationToken);

    Task<int> CreateBill(int userId, Bill bill, CancellationToken cancellationToken);

    Task<int> UpdateBill(int id, int? tripId, int? promocodeId, int? statusId, DateTime? issueDate,
        decimal? amount, decimal? remainingAmount, CancellationToken cancellationToken);

    Task ApplyPromocode(int billId, string code, CancellationToken cancellationToken);

    Task ApplyPromocode(int userId, int billId, string code, CancellationToken cancellationToken);

    Task<int> DeleteBill(int id, CancellationToken cancellationToken);
}
