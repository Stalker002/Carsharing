using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class CarStatusesService : ICarStatusesService
{
    private readonly ICarStatusRepository _carStatusRepository;

    public CarStatusesService(
        ICarStatusRepository carStatusRepository)
    {
        _carStatusRepository = carStatusRepository;
    }

    public async Task<List<CarStatus>> GetCarStatuses(CancellationToken cancellationToken)
    {
        var carStatuses = await _carStatusRepository.Get(cancellationToken);

        return carStatuses;
    }
}