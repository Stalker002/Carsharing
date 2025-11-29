using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IPromocodeRepository
{
    Task<List<Promocode>> Get();
    Task<List<Promocode>> GetPaged(int page, int limit);
    Task<int> GetCount();
    Task<List<Promocode>> GetById(int? id);
    Task<List<Promocode>> GetActive();
    Task<List<Promocode>> GetPagedActive(int page, int limit);
    Task<int> GetCountActive();
    Task<List<Promocode>> GetByCode(string code);
    Task<int> Create(Promocode promocode);
    Task<int> Update(int id, int? statusId, string? code, decimal? discount, DateOnly? startDate,
        DateOnly? endDate);
    Task<int> Delete(int id);
}