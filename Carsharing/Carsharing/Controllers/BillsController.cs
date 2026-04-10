using Carsharing.Application.Abstractions;
using Carsharing.Core.Models;
using Carsharing.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Bills;
using Shared.Contracts.Promocodes;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class BillsController : ControllerBase
{
    private readonly IBillsService _billsService;

    public BillsController(IBillsService billsService)
    {
        _billsService = billsService;
    }

    [HttpGet("unpaged")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<BillsResponse>>> GetBills(CancellationToken cancellationToken)
    {
        var bills = await _billsService.GetBills(cancellationToken);
        var response = bills.Select(b => new BillsResponse(
            b.Id,
            b.TripId,
            b.PromocodeId,
            b.StatusId,
            b.IssueDate,
            b.Amount,
            b.RemainingAmount));

        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<BillsResponse>>> GetPagedBills(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25, CancellationToken cancellationToken = default)
    {
        var totalCount = await _billsService.GetBillCount(cancellationToken);
        var reviews = await _billsService.GetPagedBills(page, limit, cancellationToken);

        var response = reviews
            .Select(b => new BillsResponse(b.Id,
                b.TripId,
                b.PromocodeId,
                b.StatusId,
                b.IssueDate,
                b.Amount,
                b.RemainingAmount)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<BillsResponse>>> GetBillById(int id, CancellationToken cancellationToken)
    {
        var bill = await _billsService.GetBillById(id, cancellationToken);
        if (bill == null)
            return NotFound("Счёт не найден");

        var response = new BillsResponse(
            bill.Id,
            bill.TripId,
            bill.PromocodeId,
            bill.StatusId,
            bill.IssueDate,
            bill.Amount,
            bill.RemainingAmount);

        return Ok(response);
    }

    [HttpGet("pagedByUser")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<BillWithMinInfoDto>>> GetPagedBillWithMinInfoByUser(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25, CancellationToken cancellationToken = default)
    {
        var userId = User.GetRequiredUserId();

        var totalCount = await _billsService.GetCountPagedBillWithMinInfoByUser(userId, cancellationToken);
        var bills = await _billsService.GetPagedBillWithMinInfoByUserId(userId, page, limit, cancellationToken);

        var response = bills
            .Select(b => new BillWithMinInfoDto(
                b.Id,
                b.StatusName,
                b.IssueDate,
                b.Amount,
                b.RemainingAmount,
                b.TariffType)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("info/{id:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<BillsResponse>>> GetBillWithInfoById(int id, CancellationToken cancellationToken)
    {
        var bill = User.IsAdmin()
            ? await _billsService.GetBillWithInfoById(id, cancellationToken)
            : await _billsService.GetBillWithInfoByUserId(User.GetRequiredUserId(), id, cancellationToken);

        if (bill.Count == 0)
            return NotFound("Счёт не найден");

        return Ok(bill);
    }

    [HttpPost]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<int>> CreateBill([FromBody] BillsRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetRequiredUserId();
        var (bill, error) = Bill.Create(
            0,
            request.TripId,
            request.PromocodeId,
            request.StatusId,
            request.IssueDate,
            request.Amount,
            request.RemainingAmount);

        if (!string.IsNullOrWhiteSpace(error)) return BadRequest(error);

        var billId = await _billsService.CreateBill(userId, bill, cancellationToken);

        return Ok(billId);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> UpdateBill(int id, [FromBody] BillsRequest request, CancellationToken cancellationToken)
    {
        var billId = await _billsService.UpdateBill(
            id,
            request.TripId,
            request.PromocodeId,
            request.StatusId,
            request.IssueDate,
            request.Amount,
            request.RemainingAmount,
            cancellationToken);
        return Ok(billId);
    }

    [HttpPost("{id:int}/promocode")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<IActionResult> ApplyPromocode(int id, [FromBody] ApplyPromocodeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (User.IsAdmin())
                await _billsService.ApplyPromocode(id, request.Code, cancellationToken);
            else
                await _billsService.ApplyPromocode(User.GetRequiredUserId(), id, request.Code, cancellationToken);
            return Ok(new { message = "Промокод применен" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeleteBill(int id, CancellationToken cancellationToken)
    {
        return Ok(await _billsService.DeleteBill(id, cancellationToken));
    }
}
