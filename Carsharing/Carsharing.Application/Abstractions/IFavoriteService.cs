using Carsharing.Application.DTOs;

namespace Carsharing.Application.Abstractions
{
    public interface IFavoriteService
    {
        Task AddToFavorites(int userId, int carId);
        Task<List<int>> GetMyFavoriteCarIds(int userId);
        Task<List<CarWithMinInfoDto>> GetMyFavoriteCarsPaged(int userId, int page, int limit);
        Task<int> GetMyFavoritesCount(int userId);
        Task RemoveFromFavorites(int userId, int carId);
    }
}