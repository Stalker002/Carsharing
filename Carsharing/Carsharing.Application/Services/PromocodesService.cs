using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class PromocodesService : IPromocodesService
{
    private readonly IPromocodeRepository _promocodeRepository;

    public PromocodesService(IPromocodeRepository promocodeRepository)
    {
        _promocodeRepository = promocodeRepository;
    }

    public async Task<List<Promocode>> GetPromocodes(CancellationToken cancellationToken)
    {
        return await _promocodeRepository.Get(cancellationToken);
    }

    public async Task<List<Promocode>> GetPagedPromocodes(int page, int limit, CancellationToken cancellationToken)
    {
        return await _promocodeRepository.GetPaged(page, limit, cancellationToken);
    }

    public async Task<int> GetCountPromocodes(CancellationToken cancellationToken)
    {
        return await _promocodeRepository.GetCount(cancellationToken);
    }

    public async Task<List<Promocode>> GetPromocodeById(int id, CancellationToken cancellationToken)
    {
        return await _promocodeRepository.GetById(id, cancellationToken);
    }

    public async Task<List<Promocode>> GetActivePromocode(CancellationToken cancellationToken)
    {
        return await _promocodeRepository.GetActive(cancellationToken);
    }

    public async Task<List<Promocode>> GetPagedActivePromocodes(int page, int limit, CancellationToken cancellationToken)
    {
        return await _promocodeRepository.GetPagedActive(page, limit, cancellationToken);
    }

    public async Task<int> GetCountActivePromocodes(CancellationToken cancellationToken)
    {
        return await _promocodeRepository.GetCountActive(cancellationToken);
    }

    public async Task<List<Promocode>> GetPromocodeByCode(string code, CancellationToken cancellationToken)
    {
        return await _promocodeRepository.GetByCode(code, cancellationToken);
    }

    public async Task<int> CreatePromocode(Promocode promocode, CancellationToken cancellationToken)
    {
        return await _promocodeRepository.Create(promocode, cancellationToken);
    }

    public async Task<int> UpdatePromocode(int id, int? statusId, string? code, decimal? discount, DateOnly? startDate,
        DateOnly? endDate, CancellationToken cancellationToken)
    {
        return await _promocodeRepository.Update(id, statusId, code, discount, startDate, endDate, cancellationToken);
    }

    public async Task<int> DeletePromocode(int id, CancellationToken cancellationToken)
    {
        return await _promocodeRepository.Delete(id, cancellationToken);
    }
}
