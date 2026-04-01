using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IInsuranceStatusRepository
{
    Task<List<InsuranceStatus>> Get(CancellationToken cancellationToken);

    Task<bool> Exists(int id, CancellationToken cancellationToken);
}