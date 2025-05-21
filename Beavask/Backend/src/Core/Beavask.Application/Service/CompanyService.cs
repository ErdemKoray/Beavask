using AutoMapper;
using Beavask.Application.Common;
using Beavask.Application.DTOs.Company;
using Beavask.Application.Interface.Service;
using Beavask.Application.Interface;
using Beavask.Application.DTOs.User;
using Beavask.Application.DTOs.Project;
using Beavask.Application.Interface.Logging;

namespace Beavask.Application.Service;

public class CompanyService(IUnitOfWork unitOfWork, IMapper mapper, IRepoService repoService, ICurrentCompanyService currentCompanyService, ILogger logger) : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IRepoService _repoService = repoService;
    private readonly ICurrentCompanyService _currentCompanyService = currentCompanyService;
    private readonly ILogger _logger = logger;

    public async Task<Response<CompanyDto>> GetByIdAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.CompanyRepository.GetByIdAsync(id);
            if (entity == null)
                return Response<CompanyDto>.NotFound();

            var dto = _mapper.Map<CompanyDto>(entity);
            return Response<CompanyDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<CompanyDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<CompanyDto>>> GetAllAsync()
    {
        try
        {
            var entities = await _unitOfWork.CompanyRepository.GetAsync();
            var dtos = _mapper.Map<IEnumerable<CompanyDto>>(entities);
            return Response<IEnumerable<CompanyDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<CompanyDto>>.Fail(ex.Message);
        }
    }

    public async Task<Response<CompanyDto>> UpdateAsync(int id, CompanyUpdateDto companyUpdateDto)
    {
        try
        {
            var existingEntity = await _unitOfWork.CompanyRepository.GetByIdAsync(id);
            if (existingEntity == null)
                return Response<CompanyDto>.NotFound();

            _mapper.Map(companyUpdateDto, existingEntity);
            await _unitOfWork.CompanyRepository.UpdateAsync(existingEntity);
            await _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<CompanyDto>(existingEntity);
            return Response<CompanyDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<CompanyDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.CompanyRepository.GetByIdAsync(id);
            if (entity == null)
                return Response<bool>.NotFound();

            await _unitOfWork.CompanyRepository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<UserBirefForCompany>>> GetAllUsersByCompanyIdAsync(int companyId)
    {
        try
        {
            var users = await _unitOfWork.UserRepository.GetAllUsersByCompanyIdAsync(companyId);
            var companyProjects = await _unitOfWork.ProjectRepository.GetAllProjectsByCompanyId(companyId);
            if (companyProjects == null || !companyProjects.Any())
            {
                return Response<IEnumerable<UserBirefForCompany>>.Success(new List<UserBirefForCompany>());
            }

            var uniqueUsernames = new HashSet<string>();
            var userBriefs = new List<UserBirefForCompany>();

            foreach (var user in users)
            {
                uniqueUsernames.Add(user.UserName);
                userBriefs.Add(new UserBirefForCompany
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    IsRegistered = true,
                    IsAssignedToCompany = true
                });
            }

            foreach(var project in companyProjects)
            {
                if (string.IsNullOrEmpty(project.RepoUrl))
                    continue;

                var contributors = await _repoService.GetRepositoryContributorsAsync(project.RepoUrl);
                if(contributors?.Data == null)
                    continue;

                foreach(var contributor in contributors.Data)
                {
                    if (string.IsNullOrEmpty(contributor.Username) || uniqueUsernames.Contains(contributor.Username))
                        continue;

                    uniqueUsernames.Add(contributor.Username);
                    var user = await _unitOfWork.UserRepository.GetSingleByConditionAsync(u => u.UserName == contributor.Username);
                    
                    if (user == null)
                    {
                        userBriefs.Add(new UserBirefForCompany
                        {
                            Username = contributor.Username,
                            Email = "",
                            IsRegistered = false,
                            IsAssignedToCompany = false
                        });
                        continue;
                    }

                    userBriefs.Add(new UserBirefForCompany
                    {
                        Id = user.Id,
                        Username = user.UserName,
                        Email = user.Email,
                        IsRegistered = true,
                        IsAssignedToCompany = user.CompanyId == companyId
                    });
                }
            }
            var sortedUserBriefs = userBriefs.OrderBy(u => u.Id).ToList();
            return Response<IEnumerable<UserBirefForCompany>>.Success(sortedUserBriefs);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<UserBirefForCompany>>.Fail($"An error occurred while fetching users: {ex.Message}");
        }
    }
    public async Task<Response<IEnumerable<UserBirefForCompany>>> GetAllUsersAccountDetailsByCompanyProjectIdAsync(int projectId)
    {
        var project = await _unitOfWork.ProjectRepository.GetByIdAsync(projectId);
        if(project == null)
            return Response<IEnumerable<UserBirefForCompany>>.NotFound();

        var projectMembers = await _unitOfWork.ProjectMemberRepository.GetAllUsersByProjectIdAsync(projectId);
        var contributors = await _repoService.GetRepositoryContributorsAsync(project.RepoUrl);

        if(contributors?.Data == null)
            return Response<IEnumerable<UserBirefForCompany>>.Success(new List<UserBirefForCompany>());

        var uniqueUsernames = new HashSet<string>();
        var userBriefs = new List<UserBirefForCompany>();        

        foreach(var member in projectMembers)
        {
            uniqueUsernames.Add(member.User.UserName);
            userBriefs.Add(new UserBirefForCompany
            {
                Id = member.User.Id,
                Username = member.User.UserName,
                Email = member.User.Email,
                IsRegistered = true,
                IsAssignedToCompany = true
            });
        }

        foreach(var contributor in contributors.Data)
        {
            if(string.IsNullOrEmpty(contributor.Username) || uniqueUsernames.Contains(contributor.Username))
                continue;

            uniqueUsernames.Add(contributor.Username);
            var user = await _unitOfWork.UserRepository.GetSingleByConditionAsync(u => u.UserName == contributor.Username);

            if(user == null)
            {
                userBriefs.Add(new UserBirefForCompany
                {
                    Username = contributor.Username,
                    Email = "",
                    IsRegistered = false,
                    IsAssignedToCompany = false
                });
                continue;
            }

            userBriefs.Add(new UserBirefForCompany
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                IsRegistered = true,
                IsAssignedToCompany = true
            });
        }
        var sortedUserBriefs = userBriefs.OrderBy(u => u.Id).ToList();
        return Response<IEnumerable<UserBirefForCompany>>.Success(sortedUserBriefs);
    }

    public async Task<Response<IEnumerable<ProjectDto>>> GetAllProjectsByCompanyIdAsync()
    {
        try
        {
            await _logger.LogInformation("Getting all projects for company {CompanyId}", _currentCompanyService.CompanyId?.ToString());
            var projects = await _unitOfWork.ProjectRepository.GetAllProjectsByCompanyId(_currentCompanyService.CompanyId ?? throw new Exception("Company not found"));
            var dtos = _mapper.Map<IEnumerable<ProjectDto>>(projects);
            await _logger.LogInformation("Successfully retrieved projects for company {CompanyId}", _currentCompanyService.CompanyId?.ToString());
            return Response<IEnumerable<ProjectDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            await _logger.LogError("Error getting projects for company", ex, _currentCompanyService.CompanyId?.ToString());
            return Response<IEnumerable<ProjectDto>>.Fail(ex.Message);
        }
    }


    public async Task<Response<IEnumerable<UserDto>>> GetAllUsersByCompanyProjectIdAsync(int projectId)
    {
        try
        {
            await _logger.LogInformation("Getting all users for project {ProjectId}", projectId.ToString());
            
            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(projectId);
            if (project == null)
            {
                await _logger.LogWarning("Project not found with id {ProjectId}", projectId.ToString());
                return Response<IEnumerable<UserDto>>.NotFound();
            }

            var users = await _unitOfWork.ProjectMemberRepository.GetAllUsersByProjectIdAsync(projectId);
            var userIds = users.Select(u => u.UserId).ToList();
            var dtos = _mapper.Map<IEnumerable<UserDto>>(users);
            
            await _logger.LogInformation("Successfully retrieved users for project {ProjectId}", projectId.ToString());
            return Response<IEnumerable<UserDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            await _logger.LogError("Error getting users for project {ProjectId}", ex, projectId.ToString());
            return Response<IEnumerable<UserDto>>.Fail(ex.Message);
        }
    }


}

