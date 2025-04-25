using Beavask.Application.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController(ICurrentUserService currentUserService) : ControllerBase
    {
        private readonly ICurrentUserService _currentUser = currentUserService;

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
                _currentUser.UserName
            });
        }
    }
}
