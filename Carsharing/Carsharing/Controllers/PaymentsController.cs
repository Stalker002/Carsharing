using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Microsoft.AspNetCore.Http.HttpResults;
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

    [HttpGet]
    public async Task<ActionResult<List<PaymentResponse>>> GetPayments()
    {
        var payments = await _paymentService.GetPayments();
        var response = payments.Select(p => new PaymentResponse(p.Id, p.BillId, p.Sum, p.Method, p.Date));

        return Ok(response);
    }

    [HttpPost]
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
    public async Task<ActionResult<int>> UpdatePayment(int id, [FromBody] PaymentRequest request)
    {
        var paymentId =
            await _paymentService.UpdatePayment(id, request.BillId, request.Sum, request.Method, request.Date);
        return Ok(paymentId);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<int>> DeletePayment(int id)
    {
        return Ok(await _paymentService.DeletePayment(id));
    }
}