using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface IPromocodeStatusesService
{
    Task<List<PromocodeStatus>> GetPromocodeStatuses();
}