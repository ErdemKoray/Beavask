using Beavask.Application.DTOs.Company;
using Beavask.Application.Interface.Service;
using Beavask.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Response<CompanyDto>>> GetById(int id)
    {
        var result = await _companyService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<CompanyDto>>>> GetAll()
    {
        var result = await _companyService.GetAllAsync();
        return Ok(result);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<Response<CompanyDto>>> Update(int id, [FromBody] CompanyUpdateDto companyUpdateDto)
    {
        var result = await _companyService.UpdateAsync(id, companyUpdateDto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Response<bool>>> Delete(int id)
    {
        var result = await _companyService.DeleteAsync(id);
        return Ok(result);
    }
    
    [HttpGet("{companyId}/users")]
    public async Task<ActionResult> GetAllUsersByCompanyIdAsync(int companyId)
    {
        var response = await _companyService.GetAllUsersByCompanyIdAsync(companyId);
        if (response.IsSuccess)
        {
            return Ok(response.Data);  
        }
        return NotFound(response.Message); 
    }

    [HttpGet("Company/projects")]
    public async Task<ActionResult> GetAllProjectsByCompanyIdAsync()
    {
        var response = await _companyService.GetAllProjectsByCompanyIdAsync();
        return Ok(response.Data);
    }

    [HttpGet("Company/projects/{projectId}/users")]
    public async Task<ActionResult> GetAllUsersByCompanyProjectIdAsync(int projectId)
    {
        var response = await _companyService.GetAllUsersByCompanyProjectIdAsync(projectId);
        if (response.IsSuccess)
        {
            return Ok(response.Data);
        }
        return NotFound(response.Message);
    }

    [HttpGet("Company/projects/{projectId}/members")]
    public async Task<ActionResult> GetAllUsersAccountDetailsByCompanyProjectIdAsync(int projectId)
    {
        var response = await _companyService.GetAllUsersAccountDetailsByCompanyProjectIdAsync(projectId);
        return Ok(response.Data);
    }
}