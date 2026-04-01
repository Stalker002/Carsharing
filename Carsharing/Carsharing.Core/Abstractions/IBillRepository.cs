using Carsharing.Application.DTOs;
using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IBillRepository
{
    Task<List<Bill>> Get(CancellationToken cancellationToken);

    Task<List<Bill>> GetPaged(int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCount(CancellationToken cancellationToken);

    Task<Bill?> GetById(int id, CancellationToken cancellationToken);

    Task<BillWithInfoDto?> GetInfoById(int id, CancellationToken cancellationToken);

    Task<List<BillWithMinInfoDto>> GetPagedMinInfoByUserId(int userId, int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCountByUserId(int userId, CancellationToken cancellationToken);

    Task<List<Bill>> GetByTripId(List<int> tripIds, CancellationToken cancellationToken);

    Task<int> Create(Bill bill, CancellationToken cancellationToken);

    Task<int> Update(int id, int? tripId, int? promocodeId, int? statusId, DateTime? issueDate,
        decimal? amount, decimal? remainingAmount, CancellationToken cancellationToken);

    Task<int> Delete(int id, CancellationToken cancellationToken);
}