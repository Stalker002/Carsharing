using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("unpaged")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<PromocodeResponse>>> GetPromocodes()
    {
        var promocodes = await _promocodesService.GetPromocodes();

        var response = promocodes.Select(pr =>
            new PromocodeResponse(pr.Id, pr.StatusId, pr.Code, pr.Discount, pr.StartDate, pr.EndDate));

        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<PromocodeResponse>>> GetPagedPromocodes(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25)
    {
        var totalCount = await _promocodesService.GetCountPromocodes();
        var promocodes = await _promocodesService.GetPagedPromocodes(page, limit);

        var response = promocodes
            .Select(pr => new PromocodeResponse(pr.Id, pr.StatusId, pr.Code, pr.Discount, pr.StartDate, pr.EndDate)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<PromocodeResponse>>> GetPromocodeById(int id)
    {
        var promocodes = await _promocodesService.GetPromocodeById(id);

        var response = promocodes.Select(pr =>
            new PromocodeResponse(pr.Id, pr.StatusId, pr.Code, pr.Discount, pr.StartDate, pr.EndDate));

        return Ok(response);
    }

    [HttpGet("Active")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<PromocodeResponse>>> GetActivePromocodes()
    {
        var promocodes = await _promocodesService.GetActivePromocode();

        var response = promocodes.Select(pr =>
            new PromocodeResponse(pr.Id, pr.StatusId, pr.Code, pr.Discount, pr.StartDate, pr.EndDate));

        return Ok(response);
    }

    [HttpGet("pagedActive")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<PromocodeResponse>>> GetPagedActivePromocodes(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25)
    {
        var totalCount = await _promocodesService.GetCountActivePromocodes();
        var promocodes = await _promocodesService.GetPagedActivePromocodes(page, limit);

        var response = promocodes
            .Select(pr => new PromocodeResponse(pr.Id, pr.StatusId, pr.Code, pr.Discount, pr.StartDate, pr.EndDate)).ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("byCode/{code}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<PromocodeResponse>>> GetPromocodeByCode(string code)
    {
        var promocodes = await _promocodesService.GetPromocodeByCode(code);

        var response = promocodes.Select(pr =>
            new PromocodeResponse(pr.Id, pr.StatusId, pr.Code, pr.Discount, pr.StartDate, pr.EndDate));

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
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
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> UpdatePromocode(int id, [FromBody] PromocodeRequest request)
    {
        var promocodeId = await _promocodesService.UpdatePromocode(id, request.StatusId, request.Code, request.Discount,
            request.StartDate, request.EndDate);

        return Ok(promocodeId);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeletePromocode(int id)
    {
        return Ok(await _promocodesService.DeletePromocode(id));
    }
}