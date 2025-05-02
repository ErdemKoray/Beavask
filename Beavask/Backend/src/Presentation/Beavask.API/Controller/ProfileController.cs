using Beavask.Application.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController(ICurrentUserService currentUserService , ICurrentCompanyService currentCompanyService) : ControllerBase
    {
        private readonly ICurrentUserService _currentUser = currentUserService;
        private readonly ICurrentCompanyService _currentCompany = currentCompanyService;

        [HttpGet("whoami")]
        public IActionResult WhoAmI()
        {
            if (_currentUser.UserId == null)
                return Unauthorized("Token içinden kullanıcı bilgisi alınamadı.");

            return Ok(new
            {
                _currentUser.UserId,
                _currentUser.Email,
                _currentUser.FirstName,
                _currentUser.LastName,
                _currentUser.UserName,
                _currentUser.AvatarUrl
            });
        }
        [HttpGet("whoami-company")]
        public IActionResult WhoAmICompany()
        {
            if (_currentCompany == null)
                return Unauthorized("Token içinden şirket bilgisi alınamadı.");

            return Ok(new
            {
                _currentCompany.CompanyId,
                _currentCompany.CompanyName,
                _currentCompany.CompanyEmail,
                _currentCompany.CompanyPhone,
                _currentCompany.CompanyDescription,
                _currentCompany.CompanyWebsite,
                _currentCompany.CompanyLogoUrl
            });
        }
    }
}
