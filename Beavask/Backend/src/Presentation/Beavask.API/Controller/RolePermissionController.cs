using Beavask.Application.DTOs.RolePermission;
using Beavask.Application.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionController : ControllerBase
    {
        private readonly IRolePermissionService _rolePermissionService;

        public RolePermissionController(IRolePermissionService rolePermissionService)
        {
            _rolePermissionService = rolePermissionService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RolePermissionCreateDto rolePermissionCreateDto)
        {
            var result = await _rolePermissionService.CreateAsync(rolePermissionCreateDto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _rolePermissionService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result);
            return NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _rolePermissionService.GetAllAsync();
            if (result.IsSuccess)
                return Ok(result);
            return NotFound(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _rolePermissionService.DeleteAsync(id);
            if (result.IsSuccess)
                return Ok(result);
            return NotFound(result);
        }
    }
}
