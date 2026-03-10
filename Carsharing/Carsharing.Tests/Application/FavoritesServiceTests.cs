using Carsharing.Application.Services;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Exceptions;
using Carsharing.Core.Models;
using Moq;

namespace Carsharing.Tests.Application;

public class FavoritesServiceTests
{
    private readonly Mock<IFavoriteRepository> _favoriteRepoMock;
    private readonly Mock<IClientRepository> _clientRepoMock;
    private readonly FavoriteService _favoriteService;

    public FavoritesServiceTests()
    {
        _favoriteRepoMock = new Mock<IFavoriteRepository>();
        _clientRepoMock = new Mock<IClientRepository>();

        _favoriteService = new FavoriteService(_favoriteRepoMock.Object, _clientRepoMock.Object);
    }

    [Fact]
    public async Task AddToFavorites_ClientNotFound_ThrowsNotFoundException()
    {
        const int userId = 1;
        const int carId = 5;

        _clientRepoMock
            .Setup(repo => repo.GetClientByUserId(userId))
            .ReturnsAsync([]);

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => 
            _favoriteService.AddToFavorites(userId, carId));

        Assert.Equal("Клиент не найден для текущего пользователя", exception.Message);
        
        _favoriteRepoMock.Verify(repo => repo.AddAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task AddToFavorites_ClientExists_CallsRepositoryAdd()
    {
        const int userId = 1;
        const int clientId = 100;
        const int carId = 5;

        var client = Client.Create(clientId, userId, "John", "Doe", "80291234567", "test@test.com").client;

        _clientRepoMock
            .Setup(repo => repo.GetClientByUserId(userId))
            .ReturnsAsync([client]);

        await _favoriteService.AddToFavorites(userId, carId);

        _favoriteRepoMock.Verify(repo => repo.AddAsync(clientId, carId), Times.Once);
    }

    [Fact]
    public async Task GetMyFavoriteCarIds_ReturnsListOfIds()
    {
        const int userId = 1;
        const int clientId = 100;
        var expectedIds = new List<int> { 5, 12, 42 };

        var client = Client.Create(clientId, userId, "John", "Doe", "80291234567", "test@test.com").client;

        _clientRepoMock
            .Setup(repo => repo.GetClientByUserId(userId))
            .ReturnsAsync([client]);

        _favoriteRepoMock
            .Setup(repo => repo.GetFavoriteCarIdsByClientIdAsync(clientId))
            .ReturnsAsync(expectedIds);

        var result = await _favoriteService.GetMyFavoriteCarIds(userId);

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal(expectedIds, result);
    }
}