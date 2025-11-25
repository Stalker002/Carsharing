using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class TariffsService : ITariffsService
{
    private readonly ITariffRepository _tariffRepository;

    public TariffsService(ITariffRepository tariffRepository)
    {
        _tariffRepository = tariffRepository;
    }

    public async Task<List<Tariff>> GetTariffs()
    {
        return await _tariffRepository.Get();
    }

    public async Task<List<Tariff>> GetTariffById(int id)
    {
        return await _tariffRepository.GetById(id);
    }

    public async Task<int> CreateTariff(Tariff tariff)
    {
        return await _tariffRepository.Create(tariff);
    }

    public async Task<int> UpdateTariff(int id, string? name, decimal? pricePerMinute, decimal? pricePerKm,
        decimal? pricePerDay)
    {
        return await _tariffRepository.Update(id, name, pricePerMinute, pricePerKm, pricePerDay);
    }

    public async Task<int> DeleteTariff(int id)
    {
        return await _tariffRepository.Delete(id);
    }
}