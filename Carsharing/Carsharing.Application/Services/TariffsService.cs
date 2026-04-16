using Carsharing.Application.Abstractions;
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

    public async Task<List<Tariff>> GetTariffs(CancellationToken cancellationToken)
    {
        return await _tariffRepository.Get(cancellationToken);
    }

    public async Task<List<Tariff>> GetTariffById(int id, CancellationToken cancellationToken)
    {
        return await _tariffRepository.GetById(id, cancellationToken);
    }

    public async Task<int> CreateTariff(Tariff tariff, CancellationToken cancellationToken)
    {
        return await _tariffRepository.Create(tariff, cancellationToken);
    }

    public async Task<int> UpdateTariff(int id, string? name, decimal? pricePerMinute, decimal? pricePerKm,
        decimal? pricePerDay, CancellationToken cancellationToken)
    {
        return await _tariffRepository.Update(id, name, pricePerMinute, pricePerKm, pricePerDay, cancellationToken);
    }

    public async Task<int> DeleteTariff(int id, CancellationToken cancellationToken)
    {
        return await _tariffRepository.Delete(id, cancellationToken);
    }
}