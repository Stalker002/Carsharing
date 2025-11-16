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