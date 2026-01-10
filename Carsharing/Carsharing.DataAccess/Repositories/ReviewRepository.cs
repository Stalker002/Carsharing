using Carsharing.Application.DTOs;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly CarsharingDbContext _context;

    public ReviewRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Review>> Get()
    {
        var reviewEntities = await _context.Review
            .AsNoTracking()
            .OrderBy(r => r.Id)
            .ToListAsync();

        var reviews = reviewEntities
            .Select(r => Review.Create(
                r.Id,
                r.ClientId,
                r.CarId,
                r.Rating,
                r.Comment,
                r.Date).review)
            .ToList();

        return reviews;
    }

    public async Task<List<Review>> GetPaged(int page, int limit)
    {
        var reviewEntities = await _context.Review
            .AsNoTracking()
            .OrderBy(r => r.Id)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        var reviews = reviewEntities
            .Select(r => Review.Create(
                r.Id,
                r.ClientId,
                r.CarId,
                r.Rating,
                r.Comment,
                r.Date).review)
            .ToList();

        return reviews;
    }

    public async Task<int> GetCount()
    {
        return await _context.Review.CountAsync();
    }

    public async Task<List<ReviewWithClientInfo>> GetByCarId(int carId)
    {
        return await _context.Review
            .AsNoTracking()
            .Where(r => r.CarId == carId)
            .OrderByDescending(r => r.Date) 
            .Select(r => new ReviewWithClientInfo(
                r.Id,
                r.Client!.Name,    
                r.Client.Surname,
                r.Rating,
                r.Comment,
                r.Date
            ))
            .ToListAsync();
    }

    public async Task<List<ReviewWithClientInfo>> GetPagedByCarId(int carId, int page, int limit)
    {
        return await _context.Review
            .AsNoTracking()
            .Where(r => r.CarId == carId)
            .OrderByDescending(r => r.Date)
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(r => new ReviewWithClientInfo(
                r.Id,
                r.Client!.Name,
                r.Client.Surname,
                r.Rating,
                r.Comment,
                r.Date
            ))
            .ToListAsync();
    }

    public async Task<int> GetCountByCarId(int carId)
    {
        return await _context.Review.Where(r => r.CarId == carId).CountAsync();
    }

    public async Task<List<Review>> GetById(int id)
    {
        var reviewEntities = await _context.Review
            .Where(r => r.Id == id)
            .AsNoTracking()
            .ToListAsync();

        var reviews = reviewEntities
            .Select(r => Review.Create(
                r.Id,
                r.ClientId,
                r.CarId,
                r.Rating,
                r.Comment,
                r.Date).review)
            .ToList();

        return reviews;
    }

    public async Task<List<Review>> GetByClientId(int clientId)
    {
        var reviewEntities = await _context.Review
            .Where(r => r.ClientId == clientId)
            .OrderBy(r => r.Id)
            .AsNoTracking()
            .ToListAsync();

        var reviews = reviewEntities
            .Select(r => Review.Create(
                r.Id,
                r.ClientId,
                r.CarId,
                r.Rating,
                r.Comment,
                r.Date).review)
            .ToList();

        return reviews;
    }

    public async Task<int> Create(Review review)
    {
        var (_, error) = Review.Create(
            0,
            review.ClientId,
            review.CarId,
            review.Rating,
            review.Comment,
            review.Date);

        if (!string.IsNullOrEmpty(error))
            throw new ArgumentException($"Create exception review: {error}");

        var reviewEntity = new ReviewEntity
        {
            ClientId = review.ClientId,
            CarId = review.CarId,
            Rating = review.Rating,
            Comment = review.Comment,
            Date = review.Date
        };

        await _context.Review.AddAsync(reviewEntity);
        await _context.SaveChangesAsync();

        return reviewEntity.Id;
    }

    public async Task<int> Update(int id, int? clientId, int? carId, short? rating, string? comment,
        DateTime? date)
    {
        var review = await _context.Review.FirstOrDefaultAsync(r => r.Id == id)
                     ?? throw new Exception("Review not found");

        if (clientId.HasValue)
            if (carId != null)
                review.CarId = carId.Value;

        if (rating.HasValue)
            review.Rating = rating.Value;

        if (string.IsNullOrWhiteSpace(comment))
            review.Comment = comment;

        if (date.HasValue)
            review.Date = date.Value;

        var (_, error) = Review.Create(
            0,
            review.ClientId,
            review.CarId,
            review.Rating,
            review.Comment,
            review.Date);

        if (!string.IsNullOrEmpty(error))
            throw new ArgumentException($"Create exception review: {error}");

        await _context.SaveChangesAsync();

        return review.Id;
    }

    public async Task<int> Delete(int id)
    {
        var reviewEntity = await _context.Review
            .Where(r => r.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}