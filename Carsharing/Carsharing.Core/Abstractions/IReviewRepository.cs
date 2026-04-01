using Carsharing.Application.DTOs;
using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IReviewRepository
{
    Task<List<Review>> Get(CancellationToken cancellationToken);

    Task<List<Review>> GetPaged(int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCount(CancellationToken cancellationToken);

    Task<List<ReviewWithClientInfo>> GetByCarId(int carId, CancellationToken cancellationToken);

    Task<List<ReviewWithClientInfo>> GetPagedByCarId(int carId, int page, int limit, CancellationToken cancellationToken);

    Task<int> GetCountByCarId(int carId, CancellationToken cancellationToken);

    Task<List<Review>> GetById(int id, CancellationToken cancellationToken);

    Task<List<Review>> GetByClientId(int clientId, CancellationToken cancellationToken);

    Task<int> Create(Review review, CancellationToken cancellationToken);

    Task<int> Update(int id, int? clientId, int? carId, short? rating, string? comment,
        DateTime? date, CancellationToken cancellationToken);

    Task<int> Delete(int id, CancellationToken cancellationToken);
}