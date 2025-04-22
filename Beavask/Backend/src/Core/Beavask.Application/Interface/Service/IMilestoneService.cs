using Beavask.Application.Common;
using Beavask.Application.DTOs.Milestone;

namespace Beavask.Application.Interface.Service;

public interface IMilestoneService
{
    Task<Response<MilestoneDto>> CreateAsync(MilestoneCreateDto milestoneCreateDto);
    Task<Response<MilestoneDto>> GetByIdAsync(int id);
    Task<Response<IEnumerable<MilestoneDto>>> GetAllAsync();
    Task<Response<MilestoneDto>> UpdateAsync(int id, MilestoneUpdateDto milestoneUpdateDto);
    Task<Response<bool>> DeleteAsync(int id);
}

