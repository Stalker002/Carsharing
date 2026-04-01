using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class InsurancesService : IInsurancesService
{
    private readonly IInsuranceRepository _insuranceRepository;

    public InsurancesService(IInsuranceRepository insuranceRepository)
    {
        _insuranceRepository = insuranceRepository;
    }

    public async Task<List<Insurance>> GetInsurances()
    {
        return await _insuranceRepository.Get();
    }

    public async Task<List<Insurance>> GetInsuranceById(int id)
    {
        return await _insuranceRepository.GetById(id);
    }

    public async Task<List<Insurance>> GetInsuranceByCarId(int carId)
    {
        return await _insuranceRepository.GetByCarId(carId);
    }

    public async Task<List<Insurance>> GetActiveInsuranceByCarId(int carId)
    {
        return await _insuranceRepository.GetActiveByCarId(carId);
    }

    public async Task<int> CreateInsurance(Insurance insurance)
    {
        return await _insuranceRepository.Create(insurance);
    }

    public async Task<int> UpdateInsurance(int id, int? carId, int? statusId, string type, string? company,
        string? policyNumber, DateOnly? startDate, DateOnly? endDate, decimal? cost)
    {
        return await _insuranceRepository.Update(id, carId, statusId, type, company, policyNumber, startDate, endDate,
            cost);
    }

    public async Task<int> DeleteInsurance(int id)
    {
        return await _insuranceRepository.Delete(id);
    }
}