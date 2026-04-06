using Shared.Contracts.Cars;

namespace Carsharing.Core.Abstractions
{
    public interface IFavoriteRepository
    {
        Task AddAsync(int clientId, int carId, CancellationToken cancellationToken);

        Task<int> GetCountByClientIdAsync(int clientId, CancellationToken cancellationToken);

        Task<List<int>> GetFavoriteCarIdsByClientIdAsync(int clientId, CancellationToken cancellationToken);

        Task<List<CarWithMinInfoDto>> GetFavoriteCarsDtoByClientIdAsync(int clientId, int page, int limit, CancellationToken cancellationToken);

        Task<bool> IsFavoriteAsync(int clientId, int carId, CancellationToken cancellationToken);

        Task RemoveAsync(int clientId, int carId, CancellationToken cancellationToken);
    }
}