using Carsharing.Application.Abstractions;
using Carsharing.Contracts;
using Carsharing.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<List<PaymentResponse>>> GetPayments(CancellationToken cancellationToken)
    {
        var payments = await _paymentService.GetPayments(cancellationToken);
        var response = payments.Select(p => new PaymentResponse(p.Id, p.BillId, p.Sum, p.Method, p.Date));

        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<PaymentResponse>>> GetPagedPayments(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25, CancellationToken cancellationToken = default)
    {
        var totalCount = await _paymentService.GetCountPayments(cancellationToken);
        var payments = await _paymentService.GetPagedPayments(page, limit, cancellationToken);

        var response = payments
            .Select(p => new PaymentResponse(p.Id, p.BillId, p.Sum, p.Method, p.Date)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<PaymentResponse>>> GetPaymentById(int id, CancellationToken cancellationToken)
    {
        var payments = await _paymentService.GetPaymentById(id, cancellationToken);
        var response = payments.Select(p => new PaymentResponse(p.Id, p.BillId, p.Sum, p.Method, p.Date));

        return Ok(response);
    }

    [HttpGet("byBill/{billId:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<PaymentResponse>>> GetPaymentByBillId(int billId, CancellationToken cancellationToken)
    {
        var payments = await _paymentService.GetPaymentByBillId(billId, cancellationToken);
        var response = payments.Select(p => new PaymentResponse(p.Id, p.BillId, p.Sum, p.Method, p.Date));

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<int>> CreatePayment([FromBody] PaymentRequest request, CancellationToken cancellationToken)
    {
        var (payment, error) = Payment.Create(
            0,
            request.BillId,
            request.Sum,
            request.Method,
            request.Date);

        if (!string.IsNullOrWhiteSpace(error)) return BadRequest(error);

        var paymentId = await _paymentService.CreatePayment(payment, cancellationToken);

        return Ok(paymentId);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> UpdatePayment(int id, [FromBody] PaymentRequest request, CancellationToken cancellationToken)
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
