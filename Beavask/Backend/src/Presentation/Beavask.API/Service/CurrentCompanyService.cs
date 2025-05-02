using System.Security.Claims;
using Beavask.Application.Interface.Service;

namespace Beavask.API.Service;

public class CurrentCompanyService(IHttpContextAccessor httpContextAccessor) : ICurrentCompanyService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public int? CompanyId => 
        int.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
            ? id : null;

    public string? CompanyName => 
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

    public string? CompanyEmail => 
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    public string? CompanyPhone => 
        _httpContextAccessor.HttpContext?.User?.FindFirstValue("CompanyPhone");

    public string? CompanyLogoUrl => 
        _httpContextAccessor.HttpContext?.User?.FindFirstValue("LogoUrl");

    public string? CompanyWebsite => 
        _httpContextAccessor.HttpContext?.User?.FindFirstValue("CompanyWebsite");

    public string? CompanyDescription => 
        _httpContextAccessor.HttpContext?.User?.FindFirstValue("CompanyDescription");
}