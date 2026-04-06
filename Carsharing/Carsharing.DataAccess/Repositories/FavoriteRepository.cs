using Carsharing.Core.Abstractions;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Cars;

namespace Carsharing.DataAccess.Repositories;

public class FavoriteRepository : IFavoriteRepository
{
    private readonly CarsharingDbContext _context;

    public FavoriteRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<int>> GetFavoriteCarIdsByClientIdAsync(int clientId, CancellationToken cancellationToken)
    {
        return await _context.Favorites
            .AsNoTracking()
            .Where(f => f.ClientId == clientId)
            .Select(f => f.CarId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<CarWithMinInfoDto>> GetFavoriteCarsDtoByClientIdAsync(int clientId, int page, int limit, CancellationToken cancellationToken)
    {
        return await _context.Favorites
            .AsNoTracking()
            .Where(f => f.ClientId == clientId)
            .Include(f => f.Car)
                .ThenInclude(c => c!.CarStatus)
            .Include(f => f.Car)
                .ThenInclude(c => c!.Tariff)
            .Include(f => f.Car)
                .ThenInclude(c => c!.SpecificationCar)
            .Include(f => f.Car)
                .ThenInclude(c => c!.Category)
            .OrderByDescending(f => f.Id)
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(f => new CarWithMinInfoDto(
                f.Car!.Id,
                f.Car.CarStatus!.Name!,
                f.Car.Tariff!.PricePerDay,
                f.Car.Category!.Name!,
                f.Car.SpecificationCar!.FuelType!,
                f.Car.SpecificationCar.MaxFuel,
                f.Car.SpecificationCar.Brand!,
                f.Car.SpecificationCar.Model!,
                f.Car.SpecificationCar.Transmission!,
                f.Car.ImagePath
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountByClientIdAsync(int clientId, CancellationToken cancellationToken)
    {
        return await _context.Favorites
            .Where(f => f.ClientId == clientId)
            .CountAsync(cancellationToken);
    }

    public async Task AddAsync(int clientId, int carId, CancellationToken cancellationToken)
    {
        var exists = await _context.Favorites
            .AnyAsync(f => f.ClientId == clientId && f.CarId == carId, cancellationToken);

        if (exists) return;

        var entity = new FavoritesEntity
        {
            ClientId = clientId,
            CarId = carId
        };

        await _context.Favorites.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(int clientId, int carId, CancellationToken cancellationToken)
    {
        await _context.Favorites
            .Where(f => f.ClientId == clientId && f.CarId == carId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<bool> IsFavoriteAsync(int clientId, int carId, CancellationToken cancellationToken)
    {
        return await _context.Favorites
            .AnyAsync(f => f.ClientId == clientId && f.CarId == carId, cancellationToken);
    }
}
