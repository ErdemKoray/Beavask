using Beavask.Application.Common;
using Beavask.Application.DTOs.Project;
using Beavask.Application.DTOs.Repo;
using Beavask.Application.DTOs.User;
using Beavask.Domain.Entities.Base;
using Beavask.Domain.Entities.Join;

namespace Beavask.Application.Interface.Service;

public interface IProjectService
{
    Task<Response<ProjectDto>> GetByIdAsync(int id);
    Task<Response<IEnumerable<ProjectDto>>> GetAllAsync();
    Task<Response<ProjectDto>> UpdateAsync(int id, ProjectUpdateDto projectUpdateDto);
    Task<Response<bool>> DeleteAsync(int id);
    Task<Response<bool>> CreateProjectFromGitHubRepoAsync(CreateProjectFromGitHubRepoDto repo, string repoUrl);
    Task<Response<List<ProjectDto>>> GetAllProjectsByUserIdAsync();
    Task<Response<List<UserDto>>> GetAllUsersByProjectIdAsync(int projectId);
}

