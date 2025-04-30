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

    [HttpPost("register")]
    public async Task<IActionResult> RegisterCompany(CompanyCreateDto companyCreateDto)
    {
        var result = await _companyService.RegisterCompanyAsync(companyCreateDto);
        if (result.IsSuccess)
            return Ok("Company successfully registered and verification code sent.");
        return BadRequest(result.Message);
    }
    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail(string email, string code)
    {
        var result = await _companyService.VerifyEmailAsync(email, code);
        if (result.IsSuccess)
            return Ok("Email successfully verified and login credentials sent.");
        return BadRequest(result.Message);
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
}
