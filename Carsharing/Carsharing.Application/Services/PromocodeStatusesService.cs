using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class PromocodeStatusesService : IPromocodeStatusesService
{
    private readonly IPromocodeStatusRepository _promocodeStatusRepository;

    public PromocodeStatusesService(
        IPromocodeStatusRepository promocodeStatusRepository)
    {
        _promocodeStatusRepository = promocodeStatusRepository;
    }

    public async Task<List<PromocodeStatus>> GetPromocodeStatuses()
    {
        var promocodeStatuses = await _promocodeStatusRepository.Get();

        return promocodeStatuses;
    }
}