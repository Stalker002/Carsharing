using Carsharing.Application.Abstractions;
using Carsharing.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Promocodes;

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
    public async Task<ActionResult<List<PromocodeResponse>>> GetPromocodes(CancellationToken cancellationToken)
    {
        var promocodes = await _promocodesService.GetPromocodes(cancellationToken);

        var response = promocodes.Select(pr =>
            new PromocodeResponse(pr.Id, pr.StatusId, pr.Code, pr.Discount, pr.StartDate, pr.EndDate));

        return Ok(response);
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<PromocodeResponse>>> GetPagedPromocodes(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25, CancellationToken cancellationToken = default)
    {
        var totalCount = await _promocodesService.GetCountPromocodes(cancellationToken);
        var promocodes = await _promocodesService.GetPagedPromocodes(page, limit, cancellationToken);

        var response = promocodes
            .Select(pr => new PromocodeResponse(pr.Id, pr.StatusId, pr.Code, pr.Discount, pr.StartDate, pr.EndDate))
            .ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<PromocodeResponse>>> GetPromocodeById(int id, CancellationToken cancellationToken)
    {
        var promocodes = await _promocodesService.GetPromocodeById(id, cancellationToken);

        var response = promocodes.Select(pr =>
            new PromocodeResponse(pr.Id, pr.StatusId, pr.Code, pr.Discount, pr.StartDate, pr.EndDate));

        return Ok(response);
    }

    [HttpGet("Active")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<PromocodeResponse>>> GetActivePromocodes(CancellationToken cancellationToken)
    {
        var promocodes = await _promocodesService.GetActivePromocode(cancellationToken);

        var response = promocodes.Select(pr =>
            new PromocodeResponse(pr.Id, pr.StatusId, pr.Code, pr.Discount, pr.StartDate, pr.EndDate));

        return Ok(response);
    }

    [HttpGet("pagedActive")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<PromocodeResponse>>> GetPagedActivePromocodes(
        [FromQuery(Name = "_page")] int page = 1,
        [FromQuery(Name = "_limit")] int limit = 25, CancellationToken cancellationToken = default)
    {
        var totalCount = await _promocodesService.GetCountActivePromocodes(cancellationToken);
        var promocodes = await _promocodesService.GetPagedActivePromocodes(page, limit, cancellationToken);

        var response = promocodes
            .Select(pr => new PromocodeResponse(pr.Id, pr.StatusId, pr.Code, pr.Discount, pr.StartDate, pr.EndDate))
            .ToList();

        Response.Headers.Append("x-total-count", totalCount.ToString());

        return Ok(response);
    }

    [HttpGet("byCode/{code}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<PromocodeResponse>>> GetPromocodeByCode(string code, CancellationToken cancellationToken)
    {
        var promocodes = await _promocodesService.GetPromocodeByCode(code, cancellationToken);

        var response = promocodes.Select(pr =>
            new PromocodeResponse(pr.Id, pr.StatusId, pr.Code, pr.Discount, pr.StartDate, pr.EndDate));

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> CreatePromocode([FromBody] PromocodeRequest request, CancellationToken cancellationToken)
    {
        var (promocode, error) = Promocode.Create(
            0,
            request.StatusId,
            request.Code,
            request.Discount,
            request.StartDate,
            request.EndDate);

        if (!string.IsNullOrWhiteSpace(error)) return BadRequest(error);

        var promocodeId = await _promocodesService.CreatePromocode(promocode, cancellationToken);

        return Ok(promocodeId);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> UpdatePromocode(int id, [FromBody] PromocodeRequest request, CancellationToken cancellationToken)
    {
        var promocodeId = await _promocodesService.UpdatePromocode(id, request.StatusId, request.Code, request.Discount,
            request.StartDate, request.EndDate, cancellationToken);

        return Ok(promocodeId);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeletePromocode(int id, CancellationToken cancellationToken)
    {
        return Ok(await _promocodesService.DeletePromocode(id, cancellationToken));
    }
}
