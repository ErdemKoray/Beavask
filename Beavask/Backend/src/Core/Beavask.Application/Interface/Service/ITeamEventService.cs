using Beavask.Application.Common;
using Beavask.Application.DTOs.TeamEvent;

namespace Beavask.Application.Interface.Service
{
    public interface ITeamEventService
    {
        Task<Response<bool>> CreateAsync(TeamEventCreateDto dto);
        Task<Response<bool>> DeleteAsync(TeamEventCreateDto dto);
    }
}
