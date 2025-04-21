using Beavask.Application.Common;
using Beavask.Application.DTOs.LogDtos;
using Beavask.Application.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogController : ControllerBase
{
    private readonly ILogService _logService;

    public LogController(ILogService logService)
    {
        _logService = logService;
    }

    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<LogDto>>>> GetAll()
    {
        var result = await _logService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Response<LogDto>>> GetById(int id)
    {
        var result = await _logService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Response<LogDto>>> Create(LogCreateDto logCreateDto)
    {
        var result = await _logService.CreateAsync(logCreateDto);
        return Ok(result);
    }
}
