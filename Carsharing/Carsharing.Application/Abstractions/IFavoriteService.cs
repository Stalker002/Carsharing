using Shared.Contracts.Cars;

namespace Carsharing.Application.Abstractions
{
    public interface IFavoriteService
    {
        Task AddToFavorites(int userId, int carId, CancellationToken cancellationToken);

        Task<List<int>> GetMyFavoriteCarIds(int userId, CancellationToken cancellationToken);

        Task<List<CarWithMinInfoDto>> GetMyFavoriteCarsPaged(int userId, int page, int limit, CancellationToken cancellationToken);

        Task<int> GetMyFavoritesCount(int userId, CancellationToken cancellationToken);

        Task RemoveFromFavorites(int userId, int carId, CancellationToken cancellationToken);
    }
}