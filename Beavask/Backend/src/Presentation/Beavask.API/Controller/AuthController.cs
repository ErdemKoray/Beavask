using Beavask.Application.DTOs.Auth;
using Beavask.Application.DTOs.Company;
using Beavask.Application.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService , IConfiguration configuration , ICurrentUserService currentUserService) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IConfiguration? _configuration;

    [HttpPost("login-personal")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (!result.IsSuccess)
            return Unauthorized(result);

        return Ok(result);
    }
    [HttpPost("login-company")]
    public async Task<IActionResult> LoginCompany([FromBody] CompanyLoginRequestDto dto)
    {
        var result = await _authService.LoginCompanyAsync(dto);
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
    [HttpPost("verify-personel-email")]
    public async Task<IActionResult> VerifyPersonelEmail(string email, string code)
    {
        var result = await _authService.VerifyPersonelEmailAsync(email, code);
        if (result.IsSuccess)
            return Ok("Email successfully verified and login credentials sent.");
        return BadRequest(result.Message);
    }
    [HttpPost("change-company-password")]
    public async Task<IActionResult> ChangeCompanyPassword([FromBody] ChangeCompanyPasswordRequestDto dto)
    {
        var result = await _authService.ChangeCompanyPasswordAsync(dto);
        if (result.IsSuccess)
            return Ok("Password successfully changed.");
        return BadRequest(result.Message);
    }
    [HttpPost("accept")]
    public async Task<IActionResult> AcceptInvitation([FromBody] AcceptInvitationRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
            return BadRequest("Token is required.");

        await _authService.AcceptInvitationAsync(request.Token , _currentUserService.UserId.Value);

        return Ok(new { message = "You have been added to the company." });
    }
    [HttpPost("change-user-password")]
    public async Task<IActionResult> ChangeUserPassword([FromBody] ChangeUserPasswordRequestDto dto)
    {
        var result = await _authService.ChangeUserPasswordAsync(dto);
        if (result.IsSuccess)
            return Ok("Password successfully changed.");
        return BadRequest(result.Message);
    }
    [HttpPost("send-reset-password-email")]
    public async Task<IActionResult> SendResetPasswordEmail(string email)
    {
        var result = await _authService.SendResetPasswordEmailAsync(email);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpPost("verify-reset-password")]
    public async Task<IActionResult> VerifyResetPassword(string email, string code)
    {
        var result = await _authService.VerifyResetPasswordAsync(email, code);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest dto)
    {
        var result = await _authService.ForgotPasswordAsync(dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}

