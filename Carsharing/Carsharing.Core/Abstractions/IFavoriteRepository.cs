using Carsharing.Application.DTOs;

namespace Carsharing.Core.Abstractions
{
    public interface IFavoriteRepository
    {
        Task AddAsync(int clientId, int carId);
        Task<int> GetCountByClientIdAsync(int clientId);
        Task<List<int>> GetFavoriteCarIdsByClientIdAsync(int clientId);
        Task<List<CarWithMinInfoDto>> GetFavoriteCarsDtoByClientIdAsync(int clientId, int page, int limit);
        Task<bool> IsFavoriteAsync(int clientId, int carId);
        Task RemoveAsync(int clientId, int carId);
    }
}