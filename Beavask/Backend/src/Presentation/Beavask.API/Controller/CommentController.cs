using Beavask.Application.Common;
using Beavask.Application.DTOs.Comment;
using Beavask.Application.Interface.Service;
using Beavask.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly ICurrentUserService _currentUserService;

    public CommentController(ICommentService commentService, ICurrentUserService currentUserService)
    {
        _commentService = commentService;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<CommentDto>>>> GetAll()
    {
        var result = await _commentService.GetAllCommentsByUserIdAsync(_currentUserService.UserId ?? throw new Exception("User not found"));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Response<CommentDto>>> GetById(int id)
    {
        var result = await _commentService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Response<CommentDto>>> Create(CommentCreateDto commentDto)
    {
        var result = await _commentService.CreateAsync(commentDto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Response<CommentDto>>> Update(int id, CommentDto commentDto)
    {
        var result = await _commentService.UpdateAsync(id, commentDto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Response<bool>>> Delete(int id)
    {
        var result = await _commentService.DeleteAsync(id);
        return Ok(result);
    }
}
