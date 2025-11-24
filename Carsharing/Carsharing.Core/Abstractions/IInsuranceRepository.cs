using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IInsuranceRepository
{
    Task<List<Insurance>> Get();
    Task<List<Insurance>> GetById(int id);
    Task<List<Insurance>> GetByCarId(int carId);
    Task<List<Insurance>> GetActiveByCarId(int carId);
    Task<int> Create(Insurance insurance);

    Task<int> Update(int id, int? carId, int? statusId, string type, string? company, string? policyNumber,
        DateOnly? startDate, DateOnly? endDate, decimal? cost);

    Task<int> Delete(int id);
}