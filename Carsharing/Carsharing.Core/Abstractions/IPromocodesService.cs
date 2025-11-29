using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IPromocodesService
{
    Task<List<Promocode>> GetPromocodes();
    Task<List<Promocode>> GetPagedPromocodes(int page, int limit);
    Task<int> GetCountPromocodes();
    Task<List<Promocode>> GetPromocodeById(int id);
    Task<List<Promocode>> GetActivePromocode();
    Task<List<Promocode>> GetPagedActivePromocodes(int page, int limit);
    Task<int> GetCountActivePromocodes();
    Task<List<Promocode>> GetPromocodeByCode(string code);
    Task<int> CreatePromocode(Promocode promocode);
    Task<int> UpdatePromocode(int id, int? statusId, string? code, decimal? discount, DateOnly? startDate,
        DateOnly? endDate);
    Task<int> DeletePromocode(int id);
}