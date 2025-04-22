using Beavask.Application.Common;
using Beavask.Application.DTOs.Project;

namespace Beavask.Application.Interface.Service;

public interface IProjectService
{
    Task<Response<ProjectDto>> GetByIdAsync(int id);
    Task<Response<IEnumerable<ProjectDto>>> GetAllAsync();
    Task<Response<ProjectDto>> CreateAsync(ProjectCreateDto projectCreateDto);
    Task<Response<ProjectDto>> UpdateAsync(int id, ProjectUpdateDto projectUpdateDto);
    Task<Response<bool>> DeleteAsync(int id);
}

