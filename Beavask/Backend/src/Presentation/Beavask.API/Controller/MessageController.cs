using Beavask.Application.Interface.Service;
using Beavask.Application.DTOs.Message;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet("sender/{senderId}")]
    public async Task<IActionResult> GetMessagesBySenderId(int senderId)
    {
        var result = await _messageService.GetMessagesBySenderIdAsync(senderId);
        if (!result.IsSuccess)
            return NotFound(result.Message);
        return Ok(result.Data);
    }

    [HttpGet("receiver/{receiverId}")]
    public async Task<IActionResult> GetMessagesByReceiverId(int receiverId)
    {
        var result = await _messageService.GetMessagesByReceiverIdAsync(receiverId);
        if (!result.IsSuccess)
            return NotFound(result.Message);
        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MessageCreateDto messageCreateDto)
    {
        var result = await _messageService.CreateAsync(messageCreateDto);
        if (!result.IsSuccess)
            return BadRequest(result.Message);
        return CreatedAtAction(nameof(GetMessagesBySenderId), new { senderId = result.Data.SenderId }, result.Data);
    }

    [HttpGet("friend/{friendId}")]
    public async Task<IActionResult> GetMessagesByFriendId(int friendId)
    {
        var result = await _messageService.GetMessagesByFriendIdIdAsync(friendId);
        if (!result.IsSuccess)
            return NotFound(result.Message);
        return Ok(result.Data);
    }
}

