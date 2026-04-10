using Carsharing.Application.Abstractions;
using Carsharing.Core.Models;
using Carsharing.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Payments;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet("unpaged")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<PaymentsResponse>>> GetPayments(CancellationToken cancellationToken)
    {
        var payments = await _paymentService.GetPayments(cancellationToken);
        var response = payments.Select(p => new PaymentsResponse(p.Id, p.BillId, p.Sum, p.Method, p.Date));

        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<PaymentsResponse>>> GetPagedPayments(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25, CancellationToken cancellationToken = default)
    {
        var totalCount = await _paymentService.GetCountPayments(cancellationToken);
        var payments = await _paymentService.GetPagedPayments(page, limit, cancellationToken);

        var response = payments
            .Select(p => new PaymentsResponse(p.Id, p.BillId, p.Sum, p.Method, p.Date)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<PaymentsResponse>>> GetPaymentById(int id, CancellationToken cancellationToken)
    {
        var payments = await _paymentService.GetPaymentById(id, cancellationToken);
        var response = payments.Select(p => new PaymentsResponse(p.Id, p.BillId, p.Sum, p.Method, p.Date));

        return Ok(response);
    }

    [HttpGet("byBill/{billId:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<PaymentsResponse>>> GetPaymentByBillId(int billId, CancellationToken cancellationToken)
    {
        var payments = User.IsAdmin()
            ? await _paymentService.GetPaymentByBillId(billId, cancellationToken)
            : await _paymentService.GetPaymentByBillId(User.GetRequiredUserId(), billId, cancellationToken);
        var response = payments.Select(p => new PaymentsResponse(p.Id, p.BillId, p.Sum, p.Method, p.Date));

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<int>> CreatePayment([FromBody] PaymentsRequest request, CancellationToken cancellationToken)
    {
        var (payment, error) = Payment.Create(
            0,
            request.BillId,
            request.Sum,
            request.Method,
            request.Date);

        if (!string.IsNullOrWhiteSpace(error)) return BadRequest(error);

        var paymentId = User.IsAdmin()
            ? await _paymentService.CreatePayment(payment, cancellationToken)
            : await _paymentService.CreatePayment(User.GetRequiredUserId(), payment, cancellationToken);

        return Ok(paymentId);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> UpdatePayment(int id, [FromBody] PaymentsRequest request, CancellationToken cancellationToken)
    {
        var paymentId =
            await _paymentService.UpdatePayment(id, request.BillId, request.Sum, request.Method, request.Date, cancellationToken);
        return Ok(paymentId);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeletePayment(int id, CancellationToken cancellationToken)
    {
        return Ok(await _paymentService.DeletePayment(id, cancellationToken));
    }
}
