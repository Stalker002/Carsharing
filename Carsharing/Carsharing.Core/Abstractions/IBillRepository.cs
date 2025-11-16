using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IBillRepository
{
    Task<List<Bill>> Get();
    Task<int> Create(Bill bill);

    Task<int> Update(int id, int? tripId, int? promocodeId, int? statusId, DateTime? issueDate,
        decimal? amount, decimal? remainingAmount);

    Task<int> Delete(int id);
}