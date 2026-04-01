using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class FinesService : IFinesService
{
    private readonly IFineRepository _fineRepository;

    public FinesService(IFineRepository fineRepository)
    {
        _fineRepository = fineRepository;
    }

    public async Task<List<Fine>> GetFines(CancellationToken cancellationToken)
    {
        return await _fineRepository.Get(cancellationToken);
    }

    public async Task<List<Fine>> GetPagedFines(int page, int limit, CancellationToken cancellationToken)
    {
        return await _fineRepository.GetPaged(page, limit, cancellationToken);
    }

    public async Task<int> GetCountFines(CancellationToken cancellationToken)
    {
        return await _fineRepository.GetCount(cancellationToken);
    }

    public async Task<List<Fine>> GetFineById(int id, CancellationToken cancellationToken)
    {
        return await _fineRepository.GetById(id, cancellationToken);
    }

    public async Task<List<Fine>> GetFinesByTripId(int tripId, CancellationToken cancellationToken)
    {
        return await _fineRepository.GetByTripId(tripId, cancellationToken);
    }

    public async Task<List<Fine>> GetFinesByStatusId(int statusId, CancellationToken cancellationToken)
    {
        return await _fineRepository.GetByStatusId(statusId, cancellationToken);
    }

    public async Task<int> CreateFine(Fine fine, CancellationToken cancellationToken)
    {
        return await _fineRepository.Create(fine, cancellationToken);
    }

    public async Task<int> UpdateFine(int id, int? tripId, int? statusId, string? type, decimal? amount,
        DateTime? date, CancellationToken cancellationToken)
    {
        return await _fineRepository.Update(id, tripId, statusId, type, amount, date, cancellationToken);
    }

    public async Task<int> DeleteFine(int id, CancellationToken cancellationToken)
    {
        return await _fineRepository.Delete(id, cancellationToken);
    }
}
