using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface IFineStatusesService
{
    Task<List<FineStatus>> GetFineStatuses(CancellationToken cancellationToken);
}