using Beavask.Application.DTOs.Role;
using Beavask.Application.DTOs.UserRole;
using Beavask.Application.Interface.Service;
using Microsoft.AspNetCore.Mvc;


namespace Beavask.API.Controller;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _roleService.GetAllAsync();
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _roleService.GetByIdAsync(id);
        if (result.IsSuccess)
            return Ok(result);
        return NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoleCreateDto roleCreateDto)
    {
        var result = await _roleService.CreateAsync(roleCreateDto);
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        return BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RoleUpdateDto roleUpdateDto)
    {
        var result = await _roleService.UpdateAsync(id, roleUpdateDto);
        if (result.IsSuccess)
            return Ok(result);
        return NotFound(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _roleService.DeleteAsync(id);
        if (result.IsSuccess)
            return NoContent();
        return NotFound(result);
    }

    [HttpPost("assign-role-to-user")]
    public async Task<IActionResult> AssignRoleToUser([FromBody] UserRoleCreateDto dto)
    {
        var result = await _roleService.AssignRoleToUserAsync(dto.UserId, dto.RoleId);
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }
}

