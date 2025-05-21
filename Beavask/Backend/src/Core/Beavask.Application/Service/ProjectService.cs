using AutoMapper;
using Beavask.Application.Common;
using Beavask.Application.DTOs.Project;
using Beavask.Application.Interface.Service;
using Beavask.Application.Interface;
using Beavask.Domain.Entities.Base;
using Beavask.Application.DTOs.Repo;
using Beavask.Application.Helper;
using Beavask.Domain.Entities.Join;

namespace Beavask.Application.Service;

public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRepoService _repoService;
    private readonly ICurrentUserService _currentUser;
    private readonly ICurrentCompanyService _currentCompany;
    public ProjectService(IUnitOfWork unitOfWork, IMapper mapper, IRepoService repoService, ICurrentUserService currentUser, ICurrentCompanyService currentCompany)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _repoService = repoService;
        _currentUser = currentUser;
        _currentCompany = currentCompany;
    }
    

    public async Task<Response<ProjectDto>> GetByIdAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
            if (entity == null)
                return Response<ProjectDto>.NotFound();

            var dto = _mapper.Map<ProjectDto>(entity);
            return Response<ProjectDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<ProjectDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<ProjectDto>>> GetAllAsync()
    {
        try
        {
            var entities = await _unitOfWork.ProjectRepository.GetAsync();
            var dtos = _mapper.Map<IEnumerable<ProjectDto>>(entities);
            return Response<IEnumerable<ProjectDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<ProjectDto>>.Fail(ex.Message);
        }
    }

    public async Task<Response<ProjectDto>> UpdateAsync(int id, ProjectUpdateDto projectUpdateDto)
    {
        try
        {
            var existing = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
            if (existing == null)
                return Response<ProjectDto>.NotFound();

            _mapper.Map(projectUpdateDto, existing);
            await _unitOfWork.ProjectRepository.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<ProjectDto>(existing);
            return Response<ProjectDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<ProjectDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
            if (entity == null)
                return Response<bool>.NotFound();

            await _unitOfWork.ProjectRepository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail(ex.Message);
        }
    }
    public async Task<Response<bool>> CreateProjectFromGitHubRepoAsync(CreateProjectFromGitHubRepoDto repo, string repoUrl)
    {
        try
        {
            bool isCompanyProject = false;

            if (_currentUser.UserId.HasValue && _currentUser.LastName != null)
            {
                isCompanyProject = false;
            }
            else if (_currentCompany.CompanyId.HasValue)
            {
                isCompanyProject = true;
            }

            var repoInfo = await _repoService.GetSingleRepositoryDetailAsync(repoUrl);

            if (repoInfo == null || repoInfo.Data == null)
            {
                return Response<bool>.Fail("Repository not found");
            }

            if (_currentUser.UserId.HasValue && _currentUser.LastName != null)
            {
                bool projectExistsForUser = await _unitOfWork.ProjectRepository.AskProjectNameExistsForUser(repoInfo.Data.HtmlUrl, _currentUser.UserId.Value);
                if (projectExistsForUser)
                {
                    return Response<bool>.Fail("Repository already exists for user");
                }
            }

            if (_currentCompany.CompanyId.HasValue && _currentUser.LastName == null)
            {
                bool projectExistsForCompany = await _unitOfWork.ProjectRepository.AskProjectNameExistsForCompany(repoInfo.Data.HtmlUrl, _currentCompany.CompanyId.Value);
                if (projectExistsForCompany)
                {
                    return Response<bool>.Fail("Repository already exists for company");
                }
            }

            var project = new Project
            {
                Name = repoInfo.Data.Name,
                Description = repoInfo.Data.Description,
                IsCompanyProject = isCompanyProject,
                RepoUrl = repoInfo.Data.HtmlUrl,
                UserId = isCompanyProject ? null : _currentUser.UserId,
                CompanyId = isCompanyProject ? _currentCompany.CompanyId : null
            };
            await _unitOfWork.ProjectRepository.AddAsync(project);
            await _unitOfWork.SaveChangesAsync();

            if (isCompanyProject)
            {
                var contributors = await _repoService.GetRepositoryContributorsAsync(repoInfo.Data.HtmlUrl);
                if (contributors?.Data == null)
                    return Response<bool>.Fail("Repository not found");

                foreach (var contributor in contributors.Data)
                {
                    var checkUser = await _unitOfWork.UserRepository.IsUserAlreadyAssignedToCompany(contributor.Username);
                    if(checkUser != null && checkUser.CompanyId == _currentCompany.CompanyId)
                    {
                        var user = await _unitOfWork.UserRepository.GetSingleByConditionAsync(u => u.UserName == contributor.Username);
                        if(user == null)
                            continue;
                        else
                        {
                            await _unitOfWork.ProjectMemberRepository.AddAsync(new ProjectMember
                            {
                                ProjectId = project.Id,
                                UserId = checkUser.Id,
                                User = checkUser,
                                Project = project
                            });
                            await _unitOfWork.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail($"Error: {ex.Message}");
        }
    }
}

