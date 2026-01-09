using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface IInsurancesService
{
    Task<List<Insurance>> GetInsurances();
    Task<List<Insurance>> GetInsuranceById(int id);
    Task<List<Insurance>> GetInsuranceByCarId(int carId);
    Task<List<Insurance>> GetActiveInsuranceByCarId(int carId);
    Task<int> CreateInsurance(Insurance insurance);

    Task<int> UpdateInsurance(int id, int? carId, int? statusId, string type, string? company,
        string? policyNumber, DateOnly? startDate, DateOnly? endDate, decimal? cost);

    Task<int> DeleteInsurance(int id);
}