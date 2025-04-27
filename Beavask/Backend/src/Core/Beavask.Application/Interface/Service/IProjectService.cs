using Beavask.Application.Common;
using Beavask.Application.DTOs.Project;
using Beavask.Application.DTOs.Repo;

namespace Beavask.Application.Interface.Service;

public interface IProjectService
{
    Task<Response<ProjectDto>> GetByIdAsync(int id);
    Task<Response<IEnumerable<ProjectDto>>> GetAllAsync();
    Task<Response<ProjectDto>> CreateAsync(ProjectCreateDto projectCreateDto);
    Task<Response<ProjectDto>> UpdateAsync(int id, ProjectUpdateDto projectUpdateDto);
    Task<Response<bool>> DeleteAsync(int id);
    Task<Response<bool>> CreateProjectFromSingleGitHubRepoAsync(CreateProjectFromGitHubRepoDto repo, int? userId);

}

