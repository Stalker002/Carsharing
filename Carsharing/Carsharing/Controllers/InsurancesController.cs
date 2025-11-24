using Carsharing.Contracts;
using Carsharing.Core.Abstractions;
using Carsharing.Core.Models;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<List<InsurancesResponse>>> GetInsurances()
    {
        var insurances = await _insurancesService.GetInsurances();
        var response = insurances.Select(i => new InsurancesResponse(i.Id, i.CarId, i.StatusId, i.Type, i.Company,
            i.PolicyNumber, i.StartDate, i.EndDate, i.Cost));

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<List<InsurancesResponse>>> GetInsuranceById(int id)
    {
        var insurances = await _insurancesService.GetInsuranceById(id);
        var response = insurances.Select(i => new InsurancesResponse(i.Id, i.CarId, i.StatusId, i.Type, i.Company,
            i.PolicyNumber, i.StartDate, i.EndDate, i.Cost));

        return Ok(response);
    }

    [HttpGet("byCarId/{carId:int}")]
    public async Task<ActionResult<List<InsurancesResponse>>> GetInsuranceByCarId(int carId)
    {
        var insurances = await _insurancesService.GetInsuranceByCarId(carId);
        var response = insurances.Select(i => new InsurancesResponse(i.Id, i.CarId, i.StatusId, i.Type, i.Company,
            i.PolicyNumber, i.StartDate, i.EndDate, i.Cost));

        return Ok(response);
    }

    [HttpGet("ActiveByCarId/{carId:int}")]
    public async Task<ActionResult<List<InsurancesResponse>>> GetActiveInsuranceByCarId(int carId)
    {
        var insurances = await _insurancesService.GetActiveInsuranceByCarId(carId);
        var response = insurances.Select(i => new InsurancesResponse(i.Id, i.CarId, i.StatusId, i.Type, i.Company,
            i.PolicyNumber, i.StartDate, i.EndDate, i.Cost));

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateInsurance([FromBody] InsuranceRequest request)
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

        var insuranceId = await _insurancesService.CreateInsurance(insurance);

        return Ok(insuranceId);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<int>> UpdateInsurance(int id, [FromBody] InsuranceRequest request)
    {
        var insuranceId = await _insurancesService.UpdateInsurance(id, request.CarId, request.StatusId, request.Type,
            request.Company, request.PolicyNumber, request.StartDate, request.EndDate, request.Cost);
        return Ok(insuranceId);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<int>> DeleteInsurance(int id)
    {
        return Ok(await _insurancesService.DeleteInsurance(id));
    }
}