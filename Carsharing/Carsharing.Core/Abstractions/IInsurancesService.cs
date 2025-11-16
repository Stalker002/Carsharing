using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IInsurancesService
{
    Task<List<Insurance>> GetInsurances();
    Task<int> CreateInsurance(Insurance insurance);

    Task<int> UpdateInsurance(int id, int? carId, int? statusId, string type, string? company,
        string? policyNumber, DateOnly? startDate, DateOnly? endDate, decimal? cost);

    Task<int> DeleteInsurance(int id);
}