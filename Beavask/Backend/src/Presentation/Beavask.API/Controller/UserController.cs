using Beavask.Application.DTOs.User;
using Beavask.Application.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controller;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _userService.GetUserBriefAsync(id);
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetByIdAllInfo(int id)
    {
        var result = await _userService.GetUserByIdAsync(id);
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllAsync();
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateDto userCreateDto)
    {
        var result = await _userService.CreateAsync(userCreateDto);
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto userUpdateDto)
    {
        var result = await _userService.UpdateAsync(id, userUpdateDto);
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _userService.DeleteAsync(id);
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPut("company/{id}")]
    public async Task<IActionResult> UpdateCompany(int id, [FromBody] UpdateCompanyRequest updateCompanyRequest)
    {
        var result = await _userService.UpdateUserCompanyIdAsync(id, updateCompanyRequest);
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet("username/{username}")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        var result = await _userService.GetUserByUsernameAsync(username);
        return Ok(result);
    }
}
