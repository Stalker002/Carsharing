using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class TripStatusesService : ITripStatusesService
{
    private readonly ITripStatusRepository _tripStatusRepository;

    public TripStatusesService(
        ITripStatusRepository tripStatusRepository)
    {
        _tripStatusRepository = tripStatusRepository;
    }

    public async Task<List<TripStatus>> GetTripStatuses()
    {
        var tripStatuses = await _tripStatusRepository.Get();

        return tripStatuses;
    }
}