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
    public async Task<ActionResult<List<PaymentResponse>>> GetPayments()
    {
        var payments = await _paymentService.GetPayments();
        var response = payments.Select(p => new PaymentResponse(p.Id, p.BillId, p.Sum, p.Method, p.Date));

        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<PaymentResponse>>> GetPagedPayments(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25)
    {
        var totalCount = await _paymentService.GetCountPayments();
        var payments = await _paymentService.GetPagedPayments(page, limit);

        var response = payments
            .Select(p => new PaymentResponse(p.Id, p.BillId, p.Sum, p.Method, p.Date)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<PaymentResponse>>> GetPaymentById(int id)
    {
        var payments = await _paymentService.GetPaymentById(id);
        var response = payments.Select(p => new PaymentResponse(p.Id, p.BillId, p.Sum, p.Method, p.Date));

        return Ok(response);
    }

    [HttpGet("byBill/{billId:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<PaymentResponse>>> GetPaymentByBillId(int billId)
    {
        var payments = await _paymentService.GetPaymentByBillId(billId);
        var response = payments.Select(p => new PaymentResponse(p.Id, p.BillId, p.Sum, p.Method, p.Date));

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<int>> CreatePayment([FromBody] PaymentRequest request)
    {
        var (payment, error) = Payment.Create(
            0,
            request.BillId,
            request.Sum,
            request.Method,
            request.Date);

        if (!string.IsNullOrWhiteSpace(error)) return BadRequest(error);

        var paymentId = await _paymentService.CreatePayment(payment);

        return Ok(paymentId);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> UpdatePayment(int id, [FromBody] PaymentRequest request)
    {
        var paymentId =
            await _paymentService.UpdatePayment(id, request.BillId, request.Sum, request.Method, request.Date);
        return Ok(paymentId);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeletePayment(int id)
    {
        return Ok(await _paymentService.DeletePayment(id));
    }
}