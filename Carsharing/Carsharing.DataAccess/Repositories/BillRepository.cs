using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Bills;

namespace Carsharing.DataAccess.Repositories;

public class BillRepository : IBillRepository
{
    private readonly CarsharingDbContext _context;

    public BillRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Bill>> Get(CancellationToken cancellationToken)
    {
        var billEntities = await _context.Bill
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var bills = billEntities
            .Select(b => Bill.Create(
                b.Id,
                b.TripId,
                b.PromocodeId,
                b.StatusId,
                b.IssueDate,
                b.Amount,
                b.RemainingAmount).bill)
            .ToList();

        return bills;
    }

    public async Task<List<Bill>> GetPaged(int page, int limit, CancellationToken cancellationToken)
    {
        var billEntities = await _context.Bill
            .AsNoTracking()
            .Skip((page - 1) * limit)
            .Take(limit)
            .OrderByDescending(b => b.IssueDate)
            .ThenByDescending(b => b.Id)
            .ToListAsync(cancellationToken);

        var bills = billEntities
            .Select(b => Bill.Create(
                b.Id,
                b.TripId,
                b.PromocodeId,
                b.StatusId,
                b.IssueDate,
                b.Amount,
                b.RemainingAmount).bill)
            .ToList();

        return bills;
    }

    public async Task<int> GetCount(CancellationToken cancellationToken)
    {
        return await _context.Bill.CountAsync(cancellationToken);
    }

    public async Task<Bill?> GetById(int id, CancellationToken cancellationToken)
    {
        var billEntities = await _context.Bill
            .Where(b => b.Id == id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var bills = billEntities
            .Select(b => Bill.Create(
                b.Id,
                b.TripId,
                b.PromocodeId,
                b.StatusId,
                b.IssueDate,
                b.Amount,
                b.RemainingAmount).bill)
            .FirstOrDefault();

        return bills;
    }

    public async Task<List<Bill>> GetByTripId(List<int> tripIds, CancellationToken cancellationToken)
    {
        var billEntities = await _context.Bill
            .Where(b => tripIds.Contains(b.Id))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var bills = billEntities
            .Select(b => Bill.Create(
                b.Id,
                b.TripId,
                b.PromocodeId,
                b.StatusId,
                b.IssueDate,
                b.Amount,
                b.RemainingAmount).bill)
            .ToList();

        return bills;
    }

    public async Task<BillWithInfoDto?> GetInfoById(int id, CancellationToken cancellationToken)
    {
        return await _context.Bill
            .AsNoTracking()
            .Where(b => b.Id == id)
            .Select(b => new BillWithInfoDto(
                b.Id,
                b.BillStatus!.Name!,
                b.Promocode != null ? b.Promocode.Code : null,
                b.IssueDate,
                b.Amount,
                b.RemainingAmount,
                b.Trip!.Booking!.CarId,
                b.Trip.Duration,
                b.Trip.Distance,
                b.Trip.TariffType
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<BillWithMinInfoDto>> GetPagedMinInfoByUserId(int userId, int page, int limit,
        CancellationToken cancellationToken)
    {
        return await _context.Bill
            .AsNoTracking()
            .Where(b => b.Trip!.Booking!.Client!.UserId == userId)
            .OrderByDescending(b => b.IssueDate)
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(b => new BillWithMinInfoDto(
                b.Id,
                b.BillStatus!.Name!,
                b.IssueDate,
                b.Amount,
                b.RemainingAmount,
                b.Trip!.TariffType
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountByUserId(int userId, CancellationToken cancellationToken)
    {
        return await _context.Bill
            .AsNoTracking()
            .Where(b => b.Trip!.Booking!.Client!.UserId == userId)
            .CountAsync(cancellationToken);
    }

    public async Task<int> Create(Bill bill, CancellationToken cancellationToken)
    {
        var (_, error) = Bill.Create(
            bill.Id,
            bill.TripId,
            bill.PromocodeId,
            bill.StatusId,
            bill.IssueDate,
            bill.Amount,
            bill.RemainingAmount);

        if (!string.IsNullOrWhiteSpace(error))
            throw new Exception($"Bill create exception: {error}");

        var billEntity = new BillEntity
        {
            Id = bill.Id,
            TripId = bill.TripId,
            PromocodeId = bill.PromocodeId,
            StatusId = bill.StatusId,
            IssueDate = bill.IssueDate,
            Amount = bill.Amount,
            RemainingAmount = bill.RemainingAmount
        };

        await _context.Bill.AddAsync(billEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return billEntity.Id;
    }

    public async Task<int> Update(int id, int? tripId, int? promocodeId, int? statusId, DateTime? issueDate,
        decimal? amount, decimal? remainingAmount, CancellationToken cancellationToken)
    {
        var bill = await _context.Bill.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)
                   ?? throw new Exception("Bill not found");

        if (tripId.HasValue)
            bill.TripId = tripId.Value;

        if (promocodeId.HasValue)
            bill.PromocodeId = promocodeId.Value;

        if (statusId.HasValue)
            bill.StatusId = statusId.Value;

        if (issueDate.HasValue)
            bill.IssueDate = issueDate.Value;

        if (amount.HasValue)
            bill.Amount = amount.Value;

        if (remainingAmount.HasValue)
            bill.RemainingAmount = remainingAmount.Value;

        var (_, error) = Bill.Create(
            bill.Id,
            bill.TripId,
            bill.PromocodeId,
            bill.StatusId,
            bill.IssueDate,
            bill.Amount,
            bill.RemainingAmount);

        if (!string.IsNullOrWhiteSpace(error))
            throw new ArgumentException($"Create exception bill: {error}");

        await _context.SaveChangesAsync(cancellationToken);

        return bill.Id;
    }

    public async Task<int> Delete(int id, CancellationToken cancellationToken)
    {
        await _context.Bill
            .Where(b => b.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return id;
    }
}