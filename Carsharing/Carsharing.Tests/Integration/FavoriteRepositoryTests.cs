using Carsharing.DataAccess;
using Carsharing.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Carsharing.Tests.Integration;

public class FavoriteRepositoryTests
{
    private CarsharingDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<CarsharingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
            .Options;

        var mockConfig = new Mock<IConfiguration>().Object;

        return new CarsharingDbContext(options, mockConfig);
    }

    [Fact]
    public async Task AddAsync_AddsFavoriteToDatabase()
    {
        var context = GetInMemoryDbContext();
        var repository = new FavoriteRepository(context);
        
        const int clientId = 10;
        const int carId = 5;

        await repository.AddAsync(clientId, carId, It.IsAny<CancellationToken>());

        var favoriteInDb = await context.Favorites.FirstOrDefaultAsync();
        
        Assert.NotNull(favoriteInDb);
        Assert.Equal(clientId, favoriteInDb.ClientId);
        Assert.Equal(carId, favoriteInDb.CarId);
    }
}