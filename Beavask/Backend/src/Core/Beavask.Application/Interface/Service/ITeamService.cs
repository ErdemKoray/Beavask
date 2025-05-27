using Beavask.Application.Common;
using Beavask.Application.DTOs.Event;
using Beavask.Application.DTOs.Team;
using Beavask.Application.DTOs.User;


namespace Beavask.Application.Interface.Service;

public interface ITeamService
{
    Task<Response<TeamDto>> GetByIdAsync(int id);
    Task<Response<IEnumerable<TeamDto>>> GetAllAsync();
    Task<Response<TeamDto>> CreateAsync(TeamCreateDto teamCreateDto);
    Task<Response<TeamDto>> UpdateAsync(int id, TeamUpdateDto teamUpdateDto);
    Task<Response<bool>> DeleteAsync(int id);

    Task<Response<TeamDto>> CreateTeamForCompanyAsync(TeamCreateDto teamCreateDto);
    Task<Response<TeamDto>> GetTeamByCompanyIdAsync(int companyId);
    Task<Response<IEnumerable<TeamDto>>> GetAllTeamsByCompanyIdAsync(int companyId);
    Task<Response<TeamWithMembersDto>> GetTeamWithMembersAsync(int teamId);
    Task<Response<IEnumerable<UserDto>>> GetMembersByTeamIdAsync(int teamId);
    Task<Response<IEnumerable<EventDto>>> GetEventsByTeamIdAsync(int teamId);
    Task<Response<bool>> AssignUserToTeamAsync(int teamId, int userId);
    Task<Response<TeamDto>> GetTeamByUserIdAsync();
}
