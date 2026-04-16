using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface IInsurancesService
{
    Task<List<Insurance>> GetInsurances(CancellationToken cancellationToken);

    Task<List<Insurance>> GetInsuranceById(int id, CancellationToken cancellationToken);

    Task<List<Insurance>> GetInsuranceByCarId(int carId, CancellationToken cancellationToken);

    Task<List<Insurance>> GetActiveInsuranceByCarId(int carId, CancellationToken cancellationToken);

    Task<int> CreateInsurance(Insurance insurance, CancellationToken cancellationToken);

    Task<int> UpdateInsurance(int id, int? carId, int? statusId, string type, string? company,
        string? policyNumber, DateOnly? startDate, DateOnly? endDate, decimal? cost,
        CancellationToken cancellationToken);

    Task<int> DeleteInsurance(int id, CancellationToken cancellationToken);
}