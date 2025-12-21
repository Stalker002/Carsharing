using Carsharing.Application.DTOs;
using Carsharing.Application.Services;
using Carsharing.Contracts;
using Carsharing.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<List<BillsResponse>>> GetBills()
    {
        var bills = await _billsService.GetBills();
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
        [FromQuery(Name = "_limit")] int limit = 25)
    {
        var totalCount = await _billsService.GetBillCount();
        var reviews = await _billsService.GetPagedBills(page, limit);

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
    public async Task<ActionResult<List<BillsResponse>>> GetBillById(int id)
    {
        var bills = await _billsService.GetBillById(id);
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

    [HttpGet("pagedByUser")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<BillWithMinInfoDto>>> GetPagedBillWithMinInfoByUser(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25)
    {
        var userId = int.Parse(User.FindFirst("userId")!.Value);

        var totalCount = await _billsService.GetCountPagedBillWithMinInfoByUser(userId);
        var bills = await _billsService.GetPagedBillWithMinInfoByUserId(userId, page, limit);

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
    public async Task<ActionResult<List<BillsResponse>>> GetBillWithInfoById(int id)
    {
        var bill = await _billsService.GetBillWithInfoById(id);

        if (bill.Count == 0)
            return NotFound("Счёт не найден");

        return Ok(bill);
    }

    [HttpPost]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<int>> CreateBill([FromBody] BillsRequest request)
    {
        var (bill, error) = Bill.Create(
            0,
            request.TripId,
            request.PromocodeId,
            request.StatusId,
            request.IssueDate,
            request.Amount,
            request.RemainingAmount);

        if (!string.IsNullOrWhiteSpace(error)) return BadRequest(error);

        var billId = await _billsService.CreateBill(bill);

        return Ok(billId);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> UpdateBill(int id, [FromBody] BillsRequest request)
    {
        var billId = await _billsService.UpdateBill(
            id,
            request.TripId,
            request.PromocodeId,
            request.StatusId,
            request.IssueDate,
            request.Amount,
            request.RemainingAmount);
        return Ok(billId);
    }

    [HttpPost("{id:int}/promocode")]
    [Authorize]
    public async Task<IActionResult> ApplyPromocode(int id, [FromBody] ApplyPromocodeRequest request)
    {
        try
        {
            await _billsService.ApplyPromocode(id, request.Code);
            return Ok(new { message = "Промокод применен" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeleteBill(int id)
    {
        return Ok(await _billsService.DeleteBill(id));
    }
}