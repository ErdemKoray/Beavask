using Beavask.Application.DTOs.TeamEvent;
using Beavask.Application.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamEventController(ITeamEventService teamEventService) : ControllerBase
    {
        private readonly ITeamEventService _teamEventService = teamEventService;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeamEventCreateDto dto)
        {
            var result = await _teamEventService.CreateAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] TeamEventCreateDto dto)
        {
            var result = await _teamEventService.DeleteAsync(dto);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}
