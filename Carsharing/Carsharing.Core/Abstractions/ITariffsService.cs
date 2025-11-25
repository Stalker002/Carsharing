using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface ITariffsService
{
    Task<List<Tariff>> GetTariffs();

    Task<List<Tariff>> GetTariffById(int id);

    Task<int> CreateTariff(Tariff tariff);

    Task<int> UpdateTariff(int id, string? name, decimal? pricePerMinute, decimal? pricePerKm,
        decimal? pricePerDay);

    Task<int> DeleteTariff(int id);
}