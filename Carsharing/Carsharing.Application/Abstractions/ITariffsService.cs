using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface ITariffsService
{
    Task<List<Tariff>> GetTariffs(CancellationToken cancellationToken);

    Task<List<Tariff>> GetTariffById(int id, CancellationToken cancellationToken);

    Task<int> CreateTariff(Tariff tariff, CancellationToken cancellationToken);

    Task<int> UpdateTariff(int id, string? name, decimal? pricePerMinute, decimal? pricePerKm,
        decimal? pricePerDay, CancellationToken cancellationToken);

    Task<int> DeleteTariff(int id, CancellationToken cancellationToken);
}