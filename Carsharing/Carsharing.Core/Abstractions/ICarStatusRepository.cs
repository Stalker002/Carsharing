using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface ICarStatusRepository
{
    Task<List<CarStatus>> Get(CancellationToken cancellationToken);

    Task<List<CarStatus>> GetById(int id, CancellationToken cancellationToken);

    Task<bool> Exists(int id, CancellationToken cancellationToken);
}