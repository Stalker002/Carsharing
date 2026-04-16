using Carsharing.DataAccess;
using Carsharing.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.Tests.Integration;

public class FavoriteRepositoryTests
{
    private CarsharingDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<CarsharingDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new CarsharingDbContext(options);
    }

    [Fact]
    public async Task AddAsync_AddsFavoriteToDatabase()
    {
        var context = GetInMemoryDbContext();
        var repository = new FavoriteRepository(context);

        const int clientId = 10;
        const int carId = 5;

        await repository.AddAsync(clientId, carId, CancellationToken.None);

        var favoriteInDb = await context.Favorites.FirstOrDefaultAsync();

        Assert.NotNull(favoriteInDb);
        Assert.Equal(clientId, favoriteInDb.ClientId);
        Assert.Equal(carId, favoriteInDb.CarId);
    }
}