using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IInsuranceRepository
{
    Task<List<Insurance>> Get();
    Task<int> Create(Insurance insurance);

    Task<int> Update(int id, int? carId, int? statusId, string type, string? company, string? policyNumber,
        DateOnly? startDate, DateOnly? endDate, decimal? cost);

    Task<int> Delete(int id);
}