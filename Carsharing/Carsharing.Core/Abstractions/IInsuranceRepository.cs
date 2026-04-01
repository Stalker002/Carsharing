using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IInsuranceRepository
{
    Task<List<Insurance>> Get(CancellationToken cancellationToken);

    Task<List<Insurance>> GetById(int id, CancellationToken cancellationToken);

    Task<List<Insurance>> GetByCarId(int carId, CancellationToken cancellationToken);

    Task<List<Insurance>> GetActiveByCarId(int carId, CancellationToken cancellationToken);

    Task<int> Create(Insurance insurance, CancellationToken cancellationToken);

    Task<int> Update(int id, int? carId, int? statusId, string type, string? company, string? policyNumber,
        DateOnly? startDate, DateOnly? endDate, decimal? cost, CancellationToken cancellationToken);

    Task<int> Delete(int id, CancellationToken cancellationToken);
}