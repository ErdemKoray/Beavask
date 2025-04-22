using Beavask.Application.DTOs.NotificationDtos;
using Beavask.Application.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _notificationService.GetAllAsync();
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NotificationCreateDto dto)
    {
        var result = await _notificationService.CreateAsync(dto);
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }
}
