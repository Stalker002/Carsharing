using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class PromocodesController : ControllerBase
{
    private readonly IPromocodesService _promocodesService;

    public PromocodesController(IPromocodesService promocodesService)
    {
        _promocodesService = promocodesService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PromocodeResponse>>> GetPromocodes()
    {
        var promocodes = await _promocodesService.GetPromocodes();

        var response = promocodes.Select(pr =>
            new PromocodeResponse(pr.Id, pr.StatusId, pr.Code, pr.Discount, pr.StartDate, pr.EndDate));

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<List<PromocodeResponse>>> GetPromocodeById(int id)
    {
        var promocodes = await _promocodesService.GetPromocodeById(id);

        var response = promocodes.Select(pr =>
            new PromocodeResponse(pr.Id, pr.StatusId, pr.Code, pr.Discount, pr.StartDate, pr.EndDate));

        return Ok(response);
    }

    [HttpGet("Active/")]
    public async Task<ActionResult<List<PromocodeResponse>>> GetActivePromocodes()
    {
        var promocodes = await _promocodesService.GetActivePromocode();

        var response = promocodes.Select(pr =>
            new PromocodeResponse(pr.Id, pr.StatusId, pr.Code, pr.Discount, pr.StartDate, pr.EndDate));

        return Ok(response);
    }

    [HttpGet("byCode/{code}")]
    public async Task<ActionResult<List<PromocodeResponse>>> GetPromocodeByCode(string code)
    {
        var promocodes = await _promocodesService.GetPromocodeByCode(code);

        var response = promocodes.Select(pr =>
            new PromocodeResponse(pr.Id, pr.StatusId, pr.Code, pr.Discount, pr.StartDate, pr.EndDate));

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreatePromocode([FromBody] PromocodeRequest request)
    {
        var (promocode, error) = Promocode.Create(
            0,
            request.StatusId,
            request.Code,
            request.Discount,
            request.StartDate,
            request.EndDate);

        if (!string.IsNullOrWhiteSpace(error)) return BadRequest(error);

        var promocodeId = await _promocodesService.CreatePromocode(promocode);

        return Ok(promocodeId);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<int>> UpdatePromocode(int id, [FromBody] PromocodeRequest request)
    {
        var promocodeId = await _promocodesService.UpdatePromocode(id, request.StatusId, request.Code, request.Discount,
            request.StartDate, request.EndDate);

        return Ok(promocodeId);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<int>> DeletePromocode(int id)
    {
        return Ok(await _promocodesService.DeletePromocode(id));
    }
}