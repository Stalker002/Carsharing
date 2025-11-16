using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface ITariffRepository
{
    Task<List<Tariff>> Get();
    Task<int> Create(Tariff tariff);

    Task<int> Update(int id, string? name, decimal? pricePerMinute, decimal? pricePerKm,
        decimal? pricePerDay);

    Task<int> Delete(int id);
}