using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class InsuranceStatusesService : IInsuranceStatusesService
{
    private readonly IInsuranceStatusRepository _insuranceStatusRepository;

    public InsuranceStatusesService(
        IInsuranceStatusRepository insuranceStatusRepository)
    {
        _insuranceStatusRepository = insuranceStatusRepository;
    }

    public async Task<List<InsuranceStatus>> GetInsuranceStatuses()
    {
        var insuranceStatuses = await _insuranceStatusRepository.Get();

        return insuranceStatuses;
    }
}