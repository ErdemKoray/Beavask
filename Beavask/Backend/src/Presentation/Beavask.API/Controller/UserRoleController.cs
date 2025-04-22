using Beavask.Application.DTOs.UserRole;
using Beavask.Application.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserRoleCreateDto userRoleCreateDto)
        {
            var result = await _userRoleService.CreateAsync(userRoleCreateDto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userRoleService.GetAllAsync();
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
