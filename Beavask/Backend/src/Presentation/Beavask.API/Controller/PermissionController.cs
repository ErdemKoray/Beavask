using Microsoft.AspNetCore.Mvc;
using Beavask.Application.DTOs.Permission;
using Beavask.Application.Interface.Service;

namespace Beavask.API.Controller;

[Route("api/[controller]")]
[ApiController]
public class PermissionController : ControllerBase
{
    private readonly IPermissionService _permissionService;

    public PermissionController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PermissionCreateDto permissionCreateDto)
    {
        var result = await _permissionService.CreateAsync(permissionCreateDto);
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PermissionUpdateDto permissionUpdateDto)
    {
        var result = await _permissionService.UpdateAsync(id, permissionUpdateDto);
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _permissionService.DeleteAsync(id);
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _permissionService.GetByIdAsync(id);
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _permissionService.GetAllAsync();
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }
}

