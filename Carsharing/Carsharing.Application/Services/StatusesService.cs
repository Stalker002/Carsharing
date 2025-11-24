using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class StatusesService : IStatusesService
{
    private readonly IStatusRepository _statusRepository;

    public StatusesService(IStatusRepository statusRepository)
    {
        _statusRepository = statusRepository;
    }

    public async Task<List<Status>> GetStatuses()
    {
        return await _statusRepository.Get();
    }

    public async Task<List<Status>> GetStatusById(int id)
    {
        return await _statusRepository.GetById(id);
    }
}