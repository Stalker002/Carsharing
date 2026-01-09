using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface ICarStatusesService
{
    Task<List<CarStatus>> GetCarStatuses();
}