using Beavask.Application.Common;
using Beavask.Application.DTOs.Team;


namespace Beavask.Application.Interface.Service;

public interface ITeamService
{
    Task<Response<TeamDto>> GetByIdAsync(int id);
    Task<Response<IEnumerable<TeamDto>>> GetAllAsync();
    Task<Response<TeamDto>> CreateAsync(TeamCreateDto teamCreateDto);
    Task<Response<TeamDto>> UpdateAsync(int id, TeamUpdateDto teamUpdateDto);
    Task<Response<bool>> DeleteAsync(int id);
}
