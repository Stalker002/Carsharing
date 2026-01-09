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

    public async Task<List<Promocode>> GetPromocodes()
    {
        return await _promocodeRepository.Get();
    }

    public async Task<List<Promocode>> GetPagedPromocodes(int page, int limit)
    {
        return await _promocodeRepository.GetPaged(page, limit);
    }

    public async Task<int> GetCountPromocodes()
    {
        return await _promocodeRepository.GetCount();
    }

    public async Task<List<Promocode>> GetPromocodeById(int id)
    {
        return await _promocodeRepository.GetById(id);
    }

    public async Task<List<Promocode>> GetActivePromocode()
    {
        return await _promocodeRepository.GetActive();
    }

    public async Task<List<Promocode>> GetPagedActivePromocodes(int page, int limit)
    {
        return await _promocodeRepository.GetPagedActive(page, limit);
    }

    public async Task<int> GetCountActivePromocodes()
    {
        return await _promocodeRepository.GetCountActive();
    }

    public async Task<List<Promocode>> GetPromocodeByCode(string code)
    {
        return await _promocodeRepository.GetByCode(code);
    }

    public async Task<int> CreatePromocode(Promocode promocode)
    {
        return await _promocodeRepository.Create(promocode);
    }

    public async Task<int> UpdatePromocode(int id, int? statusId, string? code, decimal? discount, DateOnly? startDate,
        DateOnly? endDate)
    {
        return await _promocodeRepository.Update(id, statusId, code, discount, startDate, endDate);
    }

    public async Task<int> DeletePromocode(int id)
    {
        return await _promocodeRepository.Delete(id);
    }
}