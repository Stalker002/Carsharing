using Carsharing.Application.Abstractions;
using Carsharing.Application.DTOs;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Exceptions;

namespace Carsharing.Application.Services;

public class FavoriteService : IFavoriteService
{
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IClientRepository _clientRepository;

    public FavoriteService(IFavoriteRepository favoriteRepository, IClientRepository clientRepository)
    {
        _favoriteRepository = favoriteRepository;
        _clientRepository = clientRepository;
    }

    private async Task<int> GetClientId(int userId)
    {
        var clients = await _clientRepository.GetClientByUserId(userId);
        var client = clients.FirstOrDefault();
        return client == null ? throw new NotFoundException("Клиент не найден для текущего пользователя") : client.Id;
    }

    public async Task<List<int>> GetMyFavoriteCarIds(int userId)
    {
        var clientId = await GetClientId(userId);
        return await _favoriteRepository.GetFavoriteCarIdsByClientIdAsync(clientId);
    }

    public async Task<List<CarWithMinInfoDto>> GetMyFavoriteCarsPaged(int userId, int page, int limit)
    {
        var clientId = await GetClientId(userId);
        return await _favoriteRepository.GetFavoriteCarsDtoByClientIdAsync(clientId, page, limit);
    }

    public async Task<int> GetMyFavoritesCount(int userId)
    {
        var clientId = await GetClientId(userId);
        return await _favoriteRepository.GetCountByClientIdAsync(clientId);
    }

    public async Task AddToFavorites(int userId, int carId)
    {
        var clientId = await GetClientId(userId);
        await _favoriteRepository.AddAsync(clientId, carId);
    }

    public async Task RemoveFromFavorites(int userId, int carId)
    {
        var clientId = await GetClientId(userId);
        await _favoriteRepository.RemoveAsync(clientId, carId);
    }
}
