using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface IPromocodesService
{
    Task<List<Promocode>> GetPromocodes(CancellationToken cancellationToken);

    Task<List<Promocode>> GetPagedPromocodes(int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCountPromocodes(CancellationToken cancellationToken);

    Task<List<Promocode>> GetPromocodeById(int id, CancellationToken cancellationToken);

    Task<List<Promocode>> GetActivePromocode(CancellationToken cancellationToken);

    Task<List<Promocode>> GetPagedActivePromocodes(int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCountActivePromocodes(CancellationToken cancellationToken);

    Task<List<Promocode>> GetPromocodeByCode(string code, CancellationToken cancellationToken);

    Task<int> CreatePromocode(Promocode promocode, CancellationToken cancellationToken);

    Task<int> UpdatePromocode(int id, int? statusId, string? code, decimal? discount, DateOnly? startDate,
        DateOnly? endDate, CancellationToken cancellationToken);

    Task<int> DeletePromocode(int id, CancellationToken cancellationToken);
}