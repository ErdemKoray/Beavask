using Beavask.Application.DTOs.Project;
using Beavask.Application.DTOs.Repo;
using Beavask.Application.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class ProjectController(IProjectService projectService, ICurrentUserService currentUserService) : ControllerBase
{
    private readonly IProjectService _projectService = projectService;
    private readonly ICurrentUserService? _currentUserService = currentUserService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _projectService.GetByIdAsync(id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _projectService.GetAllAsync();
        return result.IsSuccess ? Ok(result) : StatusCode(500, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProjectUpdateDto dto)
    {
        var result = await _projectService.UpdateAsync(id, dto);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _projectService.DeleteAsync(id);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpPost("create-from-github")]
    public async Task<IActionResult> CreateProjectFromGitHub([FromBody] CreateProjectFromGitHubRepoDto repo)
    {
        var repoUrl = repo.RepoUrl;

        var response = await _projectService.CreateProjectFromGitHubRepoAsync(repo, repoUrl);
        if (!response.IsSuccess)
            return BadRequest(response.Message);

        return Ok("Project created successfully.");
    }
    [HttpGet("get-all-projects-by-user")]
    public async Task<IActionResult> GetAllProjectsByUser()
    {
        if (_currentUserService == null)
            return Unauthorized("Current user service is not available.");

        var result = await _projectService.GetAllProjectsByUserIdAsync();
        return result.IsSuccess ? Ok(result) : StatusCode(500, result);
    }
}

