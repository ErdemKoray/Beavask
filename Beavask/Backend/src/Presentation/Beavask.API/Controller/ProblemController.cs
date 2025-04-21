using Beavask.Application.Common;
using Beavask.Application.DTOs.Problem;
using Beavask.Application.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class ProblemController : ControllerBase
{
    private readonly IProblemService _problemService;

    public ProblemController(IProblemService problemService)
    {
        _problemService = problemService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Response<ProblemDto>>> GetById(int id)
    {
        var result = await _problemService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<ProblemDto>>>> GetAll()
    {
        var result = await _problemService.GetAllAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Response<ProblemDto>>> Create([FromBody] ProblemCreateDto dto)
    {
        var result = await _problemService.CreateAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Response<ProblemDto>>> Update(int id, [FromBody] ProblemUpdateDto dto)
    {
        var result = await _problemService.UpdateAsync(id, dto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Response<bool>>> Delete(int id)
    {
        var result = await _problemService.DeleteAsync(id);
        return Ok(result);
    }
}
