using Carsharing.Core.Models;

namespace Carsharing.Application.Abstractions;

public interface ITripStatusesService
{
    Task<List<TripStatus>> GetTripStatuses();
}