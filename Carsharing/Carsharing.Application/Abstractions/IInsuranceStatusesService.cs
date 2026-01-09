using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface IInsuranceStatusesService
{
    Task<List<InsuranceStatus>> GetInsuranceStatuses();
}