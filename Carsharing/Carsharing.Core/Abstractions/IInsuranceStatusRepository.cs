using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IInsuranceStatusRepository
{
    Task<List<InsuranceStatus>> Get();
    Task<bool> Exists(int id);
}