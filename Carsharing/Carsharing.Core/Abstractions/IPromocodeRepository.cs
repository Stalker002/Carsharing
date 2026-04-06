using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IPromocodeRepository
{
    Task<List<Promocode>> Get(CancellationToken cancellationToken);

    Task<List<Promocode>> GetPaged(int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCount(CancellationToken cancellationToken);

    Task<List<Promocode>> GetById(int? id, CancellationToken cancellationToken);

    Task<List<Promocode>> GetActive(CancellationToken cancellationToken);

    Task<List<Promocode>> GetPagedActive(int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCountActive(CancellationToken cancellationToken);

    Task<List<Promocode>> GetByCode(string code, CancellationToken cancellationToken);

    Task<int> Create(Promocode promocode, CancellationToken cancellationToken);

    Task<int> Update(int id, int? statusId, string? code, decimal? discount, DateOnly? startDate,
        DateOnly? endDate, CancellationToken cancellationToken);

    Task<int> Delete(int id, CancellationToken cancellationToken);
}