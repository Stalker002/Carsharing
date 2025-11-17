using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Carsharing.Core.Models;

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

    [HttpGet]
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

    [HttpPost]
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

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<int>> DeleteBill(int id)
    {
        return Ok(await _billsService.DeleteBill(id));
    }
}