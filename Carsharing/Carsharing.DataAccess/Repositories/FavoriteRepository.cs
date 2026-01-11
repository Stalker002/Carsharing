using Carsharing.Application.DTOs;
using Carsharing.Core.Abstractions;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class FavoriteRepository : IFavoriteRepository
{
    private readonly CarsharingDbContext _context;

    public FavoriteRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<int>> GetFavoriteCarIdsByClientIdAsync(int clientId)
    {
        return await _context.Favorites
            .AsNoTracking()
            .Where(f => f.ClientId == clientId)
            .Select(f => f.CarId)
            .ToListAsync();
    }

    public async Task<List<CarWithMinInfoDto>> GetFavoriteCarsDtoByClientIdAsync(int clientId, int page, int limit)
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
            .OrderByDescending(f => f.Id) // Сначала новые лайки
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(f => new CarWithMinInfoDto(
                f.Car!.Id,
                f.Car.CarStatus!.Name,
                f.Car.Tariff!.PricePerDay,
                f.Car.Category!.Name,
                f.Car.SpecificationCar!.FuelType!,
                f.Car.SpecificationCar.MaxFuel,
                f.Car.SpecificationCar.Brand!,
                f.Car.SpecificationCar.Model!,
                f.Car.SpecificationCar.Transmission!,
                f.Car.ImagePath
            ))
            .ToListAsync();
    }

    public async Task<int> GetCountByClientIdAsync(int clientId)
    {
        return await _context.Favorites
            .Where(f => f.ClientId == clientId)
            .CountAsync();
    }

    public async Task AddAsync(int clientId, int carId)
    {
        var exists = await _context.Favorites
            .AnyAsync(f => f.ClientId == clientId && f.CarId == carId);

        if (exists) return;

        var entity = new FavoritesEntity
        {
            ClientId = clientId,
            CarId = carId
        };

        await _context.Favorites.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(int clientId, int carId)
    {
        await _context.Favorites
            .Where(f => f.ClientId == clientId && f.CarId == carId)
            .ExecuteDeleteAsync();
    }

    public async Task<bool> IsFavoriteAsync(int clientId, int carId)
    {
        return await _context.Favorites
            .AnyAsync(f => f.ClientId == clientId && f.CarId == carId);
    }
}