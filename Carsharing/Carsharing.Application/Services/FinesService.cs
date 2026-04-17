using Carsharing.Application.Abstractions;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Exceptions;
using Carsharing.Core.Models;

namespace Carsharing.Application.Services;

public class FinesService : IFinesService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IFineRepository _fineRepository;
    private readonly ITripRepository _tripRepository;

    public FinesService(
        IFineRepository fineRepository,
        ITripRepository tripRepository,
        IBookingRepository bookingRepository,
        IClientRepository clientRepository)
    {
        _tripRepository = tripRepository;
        _bookingRepository = bookingRepository;
        _clientRepository = clientRepository;
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

    public async Task<List<Fine>> GetFinesByTripId(int userId, int tripId, CancellationToken cancellationToken)
    {
        var client = (await _clientRepository.GetClientByUserId(userId, cancellationToken)).FirstOrDefault()
                     ?? throw new NotFoundException("Client not found");

        var trip = (await _tripRepository.GetById(tripId, cancellationToken)).FirstOrDefault()
                   ?? throw new NotFoundException("Trip not found");

        var booking = (await _bookingRepository.GetById(trip.BookingId, cancellationToken)).FirstOrDefault()
                      ?? throw new NotFoundException("Booking not found");

        if (booking.ClientId != client.Id)
            throw new UnauthorizedAccessException("Trip does not belong to current user");

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