using Beavask.Application.Common;
using Beavask.Application.DTOs.Event;
using Beavask.Application.DTOs.Team;
using Beavask.Application.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace Beavask.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Response<TeamDto>>> GetById(int id)
    {
        var result = await _teamService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<TeamDto>>>> GetAll()
    {
        var result = await _teamService.GetAllAsync();  
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Response<TeamDto>>> Create(TeamCreateDto teamCreate)
    {
        var result = await _teamService.CreateAsync(teamCreate);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Response<TeamDto>>> Update(int id, TeamUpdateDto teamUpdate)
    {
        var result = await _teamService.UpdateAsync(id, teamUpdate);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Response<bool>>> Delete(int id)
    {
        var result = await _teamService.DeleteAsync(id);
        return Ok(result);
    }

    [HttpGet("{teamId}/members")]
    public async Task<ActionResult<Response<TeamWithMembersDto>>> GetTeamWithMembers(int teamId)
    {
        var result = await _teamService.GetTeamWithMembersAsync(teamId);
        return Ok(result);
    }

    [HttpGet("{teamId}/members/all")]
    public async Task<ActionResult<Response<IEnumerable<TeamDto>>>> GetMembersByTeamId(int teamId)
    {
        var result = await _teamService.GetMembersByTeamIdAsync(teamId);
        return Ok(result);
    }

    [HttpGet("{teamId}/events")]
    public async Task<ActionResult<Response<IEnumerable<EventDto>>>> GetEventsByTeamId(int teamId)
    {
        var result = await _teamService.GetEventsByTeamIdAsync(teamId);
        return Ok(result);
    }

    [HttpPost("company/create-team")]
    public async Task<ActionResult<Response<TeamDto>>> CreateTeamForCompany(TeamCreateDto teamCreate)
    {
        var result = await _teamService.CreateTeamForCompanyAsync(teamCreate);
        return Ok(result);
    }

    [HttpGet("company/{companyId}")]
    public async Task<ActionResult<Response<TeamDto>>> GetTeamByCompanyId(int companyId)
    {
        var result = await _teamService.GetTeamByCompanyIdAsync(companyId);
        return Ok(result);
    }   

    [HttpPost("{teamId}/assign-user/{userId}")]
    public async Task<ActionResult<Response<bool>>> AssignUserToTeam(int teamId, int userId)
    {
        var result = await _teamService.AssignUserToTeamAsync(teamId, userId);
        return Ok(result);
    }

    [HttpGet("company/{companyId}/teams")]
    public async Task<ActionResult<Response<IEnumerable<TeamDto>>>> GetAllTeamsByCompanyId(int companyId)
    {
        var result = await _teamService.GetAllTeamsByCompanyIdAsync(companyId);
        return Ok(result);
    }

    [HttpGet("user/team")]
    public async Task<ActionResult<Response<TeamDto>>> GetTeamByUserId()
    {
        var result = await _teamService.GetTeamByUserIdAsync();
        return Ok(result);
    }
}