using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class FineStatusesService : IFineStatusesService
{
    private readonly IFineStatusRepository _fineStatusRepository;

    public FineStatusesService(
        IFineStatusRepository fineStatusRepository)
    {
        _fineStatusRepository = fineStatusRepository;
    }

    public async Task<List<FineStatus>> GetFineStatuses()
    {
        var fineStatuses = await _fineStatusRepository.Get();

        return fineStatuses;
    }
}