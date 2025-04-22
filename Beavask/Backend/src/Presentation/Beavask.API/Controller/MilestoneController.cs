using Beavask.Application.DTOs.Milestone;
using Beavask.Application.Interface;
using Beavask.Application.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controller;

[Route("api/[controller]")]
[ApiController]
public class MilestoneController(IMilestoneService milestoneService) : ControllerBase
{
    private readonly IMilestoneService _milestoneService = milestoneService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MilestoneCreateDto milestoneCreateDto)
    {
        var result = await _milestoneService.CreateAsync(milestoneCreateDto);
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _milestoneService.GetByIdAsync(id);
        if (result.IsSuccess)
            return Ok(result);
        return NotFound(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _milestoneService.GetAllAsync();
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MilestoneUpdateDto milestoneUpdateDto)
    {
        var result = await _milestoneService.UpdateAsync(id, milestoneUpdateDto);
        if (result.IsSuccess)
            return Ok(result);
        return NotFound(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _milestoneService.DeleteAsync(id);
        if (result.IsSuccess)
            return Ok(result);
        return NotFound(result);
    }
}

