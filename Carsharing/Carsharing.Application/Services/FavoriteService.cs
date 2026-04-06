using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Exceptions;
using Shared.Contracts.Cars;

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

    private async Task<int> GetClientId(int userId, CancellationToken cancellationToken)
    {
        var clients = await _clientRepository.GetClientByUserId(userId, cancellationToken);
        var client = clients.FirstOrDefault();
        return client == null ? throw new NotFoundException("Клиент не найден для текущего пользователя") : client.Id;
    }

    public async Task<List<int>> GetMyFavoriteCarIds(int userId, CancellationToken cancellationToken)
    {
        var clientId = await GetClientId(userId, cancellationToken);
        return await _favoriteRepository.GetFavoriteCarIdsByClientIdAsync(clientId, cancellationToken);
    }

    public async Task<List<CarWithMinInfoDto>> GetMyFavoriteCarsPaged(int userId, int page, int limit, CancellationToken cancellationToken)
    {
        var clientId = await GetClientId(userId, cancellationToken);
        return await _favoriteRepository.GetFavoriteCarsDtoByClientIdAsync(clientId, page, limit, cancellationToken);
    }

    public async Task<int> GetMyFavoritesCount(int userId, CancellationToken cancellationToken)
    {
        var clientId = await GetClientId(userId, cancellationToken);
        return await _favoriteRepository.GetCountByClientIdAsync(clientId, cancellationToken);
    }

    public async Task AddToFavorites(int userId, int carId, CancellationToken cancellationToken)
    {
        var clientId = await GetClientId(userId, cancellationToken);
        await _favoriteRepository.AddAsync(clientId, carId, cancellationToken);
    }

    public async Task RemoveFromFavorites(int userId, int carId, CancellationToken cancellationToken)
    {
        var clientId = await GetClientId(userId, cancellationToken);
        await _favoriteRepository.RemoveAsync(clientId, carId, cancellationToken);
    }
}
