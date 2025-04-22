using Beavask.Application.Common;
using Beavask.Application.DTOs.UserContact;
using Beavask.Application.Interface.Service;
using Beavask.Application.Interfaces;
using Beavask.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserContactController : ControllerBase
{
    private readonly IUserContactService _userContactService;

    public UserContactController(IUserContactService userContactService)
    {
        _userContactService = userContactService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Response<UserContactDto>>> GetById(int id)
    {
        var result = await _userContactService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<UserContactDto>>>> GetAll()
    {
        var result = await _userContactService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<Response<IEnumerable<UserContactDto>>>> GetByUserId(int userId)
    {
        var result = await _userContactService.GetByUserIdAsync(userId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Response<UserContactDto>>> Create(UserContactCreateDto userContactCreate)
    {
        var result = await _userContactService.CreateAsync(userContactCreate);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Response<UserContactDto>>> Update(int id, UserContactCreateDto userContactCreate)
    {
        var result = await _userContactService.UpdateAsync(id, userContactCreate);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Response<bool>>> Delete(int id)
    {
        var result = await _userContactService.DeleteAsync(id);
        return Ok(result);
    }
}
