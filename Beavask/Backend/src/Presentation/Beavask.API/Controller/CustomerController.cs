using Beavask.Application.DTOs.Customer;
using Beavask.Application.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controller;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _customerService.GetByIdAsync(id);
        if(result.IsSuccess)
            return Ok(result);
        else
            return NotFound(result.Message);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _customerService.GetAllAsync();
        if(result.IsSuccess)
            return Ok(result);
        else
            return NotFound(result.Message);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CustomerCreateDto dto)
    {
        var result = await _customerService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CustomerUpdateDto dto)
    {
        var result = await _customerService.UpdateAsync(id, dto);
        if (result.IsSuccess)
        {
            return NotFound(result.Message);
        }

        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _customerService.DeleteAsync(id);
        if (result.IsSuccess)
        {
            return NotFound(result.Message);
        }

        return NoContent();
    }
}
