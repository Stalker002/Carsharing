using Carsharing.Core.Models;

namespace Carsharing.Core.Abstractions;

public interface IStatusesService
{
    Task<List<Status>> GetStatuses();
    Task<List<Status>> GetStatusById(int id);
}