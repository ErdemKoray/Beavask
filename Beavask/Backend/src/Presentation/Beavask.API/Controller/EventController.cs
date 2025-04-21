using Beavask.Application.Common;
using Beavask.Application.DTOs.Event;
using Beavask.Application.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Response<EventDto>>> GetById(int id)
    {
        var result = await _eventService.GetEventByIdAsync(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<EventDto>>>> GetAll()
    {
        var result = await _eventService.GetAllEventsAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Response<EventDto>>> Create([FromBody] EventCreateDto dto)
    {
        var result = await _eventService.CreateEventAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Response<EventDto>>> Update(int id, [FromBody] EventUpdateDto dto)
    {
        var result = await _eventService.UpdateEventAsync(id, dto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Response<bool>>> Delete(int id)
    {
        var result = await _eventService.DeleteEventAsync(id);
        return Ok(result);
    }
}
