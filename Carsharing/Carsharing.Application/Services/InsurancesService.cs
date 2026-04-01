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

    public async Task<List<Insurance>> GetInsurances(CancellationToken cancellationToken)
    {
        return await _insuranceRepository.Get(cancellationToken);
    }

    public async Task<List<Insurance>> GetInsuranceById(int id, CancellationToken cancellationToken)
    {
        return await _insuranceRepository.GetById(id, cancellationToken);
    }

    public async Task<List<Insurance>> GetInsuranceByCarId(int carId, CancellationToken cancellationToken)
    {
        return await _insuranceRepository.GetByCarId(carId, cancellationToken);
    }

    public async Task<List<Insurance>> GetActiveInsuranceByCarId(int carId, CancellationToken cancellationToken)
    {
        return await _insuranceRepository.GetActiveByCarId(carId, cancellationToken);
    }

    public async Task<int> CreateInsurance(Insurance insurance, CancellationToken cancellationToken)
    {
        return await _insuranceRepository.Create(insurance, cancellationToken);
    }

    public async Task<int> UpdateInsurance(int id, int? carId, int? statusId, string type, string? company,
        string? policyNumber, DateOnly? startDate, DateOnly? endDate, decimal? cost, CancellationToken cancellationToken)
    {
        return await _insuranceRepository.Update(id, carId, statusId, type, company, policyNumber, startDate, endDate,
            cost, cancellationToken);
    }

    public async Task<int> DeleteInsurance(int id, CancellationToken cancellationToken)
    {
        return await _insuranceRepository.Delete(id, cancellationToken);
    }
}
