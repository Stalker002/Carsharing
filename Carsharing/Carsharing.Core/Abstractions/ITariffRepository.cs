using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface ITariffRepository
{
    Task<List<Tariff>> Get(CancellationToken cancellationToken);

    Task<List<Tariff>> GetById(int id, CancellationToken cancellationToken);

    Task<int> Create(Tariff tariff, CancellationToken cancellationToken);

    Task<int> Update(int id, string? name, decimal? pricePerMinute, decimal? pricePerKm,
        decimal? pricePerDay, CancellationToken cancellationToken);

    Task<int> Delete(int id, CancellationToken cancellationToken);
}