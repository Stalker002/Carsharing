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

    public async Task<List<Fine>> GetFines()
    {
        return await _fineRepository.Get();
    }

    public async Task<List<Fine>> GetPagedFines(int page, int limit)
    {
        return await _fineRepository.GetPaged(page, limit);
    }

    public async Task<int> GetCountFines()
    {
        return await _fineRepository.GetCount();
    }

    public async Task<List<Fine>> GetFineById(int id)
    {
        return await _fineRepository.GetById(id);
    }

    public async Task<List<Fine>> GetFinesByTripId(int tripId)
    {
        return await _fineRepository.GetByTripId(tripId);
    }

    public async Task<List<Fine>> GetFinesByStatusId(int statusId)
    {
        return await _fineRepository.GetByStatusId(statusId);
    }

    public async Task<int> CreateFine(Fine fine)
    {
        return await _fineRepository.Create(fine);
    }

    public async Task<int> UpdateFine(int id, int? tripId, int? statusId, string? type, decimal? amount,
        DateTime? date)
    {
        return await _fineRepository.Update(id, tripId, statusId, type, amount, date);
    }

    public async Task<int> DeleteFine(int id)
    {
        return await _fineRepository.Delete(id);
    }
}