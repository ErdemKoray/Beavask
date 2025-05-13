using Beavask.Application.DTOs.Task;
using Beavask.Application.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class TaskController(ITaskService taskService) : ControllerBase
{
    private readonly ITaskService _taskService = taskService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _taskService.GetByIdAsync(id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _taskService.GetAllAsync();
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TaskCreateDto dto)
    {
        var result = await _taskService.CreateAsync(dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TaskUpdateDto dto)
    {
        var result = await _taskService.UpdateAsync(id, dto);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _taskService.DeleteAsync(id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
    [HttpPost("{taskId}/assign/{userId}")]
    public async Task<IActionResult> AssignToUser(int taskId, int userId)
    {
        var result = await _taskService.AssigneToUserAsync(taskId, userId);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetAllTaskByProjectId(int projectId)
    {
        var result = await _taskService.GetAllTaskByProjectIdAsync(projectId);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetAllTaskByUserId(int userId)
    {
        var result = await _taskService.GetAllTaskByUserIdAsync(userId);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}

