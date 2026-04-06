using Carsharing.Application.Abstractions;
using Carsharing.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Insurances;

namespace Carsharing.Controllers;

[ApiController]
[Route("[controller]")]
public class InsurancesController : ControllerBase
{
    private readonly IInsurancesService _insurancesService;

    public InsurancesController(IInsurancesService insurancesService)
    {
        _insurancesService = insurancesService;
    }

    [HttpGet]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<InsurancesResponse>>> GetInsurances(CancellationToken cancellationToken)
    {
        var insurances = await _insurancesService.GetInsurances(cancellationToken);
        var response = insurances.Select(i => new InsurancesResponse(i.Id, i.CarId, i.StatusId, i.Type, i.Company,
            i.PolicyNumber, i.StartDate, i.EndDate, i.Cost));

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<InsurancesResponse>>> GetInsuranceById(int id, CancellationToken cancellationToken)
    {
        var insurances = await _insurancesService.GetInsuranceById(id, cancellationToken);
        var response = insurances.Select(i => new InsurancesResponse(i.Id, i.CarId, i.StatusId, i.Type, i.Company,
            i.PolicyNumber, i.StartDate, i.EndDate, i.Cost));

        return Ok(response);
    }

    [HttpGet("byCarId/{carId:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<List<InsurancesResponse>>> GetInsuranceByCarId(int carId, CancellationToken cancellationToken)
    {
        var insurances = await _insurancesService.GetInsuranceByCarId(carId, cancellationToken);
        var response = insurances.Select(i => new InsurancesResponse(i.Id, i.CarId, i.StatusId, i.Type, i.Company,
            i.PolicyNumber, i.StartDate, i.EndDate, i.Cost));

        return Ok(response);
    }

    [HttpGet("ActiveByCarId/{carId:int}")]
    [Authorize(Policy = "AdminClientPolicy")]
    public async Task<ActionResult<List<InsurancesResponse>>> GetActiveInsuranceByCarId(int carId, CancellationToken cancellationToken)
    {
        var insurances = await _insurancesService.GetActiveInsuranceByCarId(carId, cancellationToken);
        var response = insurances.Select(i => new InsurancesResponse(i.Id, i.CarId, i.StatusId, i.Type, i.Company,
            i.PolicyNumber, i.StartDate, i.EndDate, i.Cost));

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> CreateInsurance([FromBody] InsurancesRequest request, CancellationToken cancellationToken)
    {
        var (insurance, error) = Insurance.Create(
            0,
            request.CarId,
            request.StatusId,
            request.Type,
            request.Company,
            request.PolicyNumber,
            request.StartDate,
            request.EndDate,
            request.Cost);

        if (!string.IsNullOrWhiteSpace(error)) return BadRequest(error);

        var insuranceId = await _insurancesService.CreateInsurance(insurance, cancellationToken);

        return Ok(insuranceId);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> UpdateInsurance(int id, [FromBody] InsurancesRequest request, CancellationToken cancellationToken)
    {
        var insuranceId = await _insurancesService.UpdateInsurance(id, request.CarId, request.StatusId, request.Type,
            request.Company, request.PolicyNumber, request.StartDate, request.EndDate, request.Cost, cancellationToken);
        return Ok(insuranceId);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult<int>> DeleteInsurance(int id, CancellationToken cancellationToken)
    {
        return Ok(await _insurancesService.DeleteInsurance(id, cancellationToken));
    }
}
