using Carsharing.Application.DTOs;
using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IBillRepository
{
    Task<List<Bill>> Get();
    Task<List<Bill>> GetPaged(int page, int limit);
    Task<int> GetCount();
    Task<Bill?> GetById(int id);
    Task<BillWithInfoDto?> GetInfoById(int id);
    Task<List<BillWithMinInfoDto>> GetPagedMinInfoByUserId(int userId, int page, int limit);
    Task<int> GetCountByUserId(int userId);
    Task<List<Bill>> GetByTripId(List<int> tripIds);
    Task<int> Create(Bill bill);
    Task<int> Update(int id, int? tripId, int? promocodeId, int? statusId, DateTime? issueDate,
        decimal? amount, decimal? remainingAmount);
    Task<int> Delete(int id);
}