using Beavask.Application.DTOs.Auth;
using Beavask.Application.DTOs.Company;
using Beavask.Application.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService , IConfiguration configuration) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly IConfiguration? _configuration;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (!result.IsSuccess)
            return Unauthorized(result);

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("github-login")]
    public async Task<IActionResult> GitHubLogin([FromBody] GitHubLoginRequestDto dto)
    {
        var clientId = configuration["GitHub:ClientId"];
        var clientSecret = configuration["GitHub:ClientSecret"];

        var result = await _authService.LoginWithGitHubAsync(dto, clientId!, clientSecret!);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpPost("company-register")]
    public async Task<IActionResult> RegisterCompany(CompanyCreateDto companyCreateDto)
    {
        var result = await _authService.RegisterCompanyAsync(companyCreateDto);
        if (result.IsSuccess)
            return Ok("Company successfully registered and verification code sent.");
        return BadRequest(result.Message);
    }
    [HttpPost("verify-company-email")]
    public async Task<IActionResult> VerifyEmail(string email, string code)
    {
        var result = await _authService.VerifyCompanyEmailAsync(email, code);
        if (result.IsSuccess)
            return Ok("Email successfully verified and login credentials sent.");
        return BadRequest(result.Message);
    }
}

