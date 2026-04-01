using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public interface IFineStatusesService
{
    Task<List<FineStatus>> GetFineStatuses();
}