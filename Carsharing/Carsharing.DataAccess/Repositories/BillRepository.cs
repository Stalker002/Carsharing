using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Carsharing.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;

namespace Carsharing.DataAccess.Repositories;

public class BillRepository : IBillRepository
{
    private readonly CarsharingDbContext _context;

    public BillRepository(CarsharingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Bill>> Get()
    {
        var billEntities = await _context.Bill
            .AsNoTracking()
            .ToListAsync();

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

    public async Task<List<Bill>> GetById(int id)
    {
        var billEntities = await _context.Bill
            .Where(b => b.Id == id)
            .AsNoTracking()
            .ToListAsync();

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

    public async Task<List<Bill>> GetByTripId(int tripId)
    {
        var billEntities = await _context.Bill
            .Where(b => b.TripId == tripId)
            .AsNoTracking()
            .ToListAsync();

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

    public async Task<List<Bill>> GetByTripId(List<int> tripIds)
    {
        var billEntities = await _context.Bill
            .Where(b => tripIds.Contains(b.Id))
            .AsNoTracking()
            .ToListAsync();

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

    public async Task<int> Create(Bill bill)
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

        await _context.Bill.AddAsync(billEntity);
        await _context.SaveChangesAsync();

        return billEntity.Id;
    }

    public async Task<int> Update(int id, int? tripId, int? promocodeId, int? statusId, DateTime? issueDate,
        decimal? amount, decimal? remainingAmount)
    {
        var bill = await _context.Bill.FirstOrDefaultAsync(b => b.Id == id)
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

            await _context.SaveChangesAsync();

        return bill.Id;
    }

    public async Task<int> Delete(int id)
    {
        var billEntity = await _context.Bill
            .Where(b => b.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}