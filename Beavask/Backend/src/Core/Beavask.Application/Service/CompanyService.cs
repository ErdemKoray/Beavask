using AutoMapper;
using Beavask.Application.Common;
using Beavask.Application.DTOs.Company;
using Beavask.Application.Interface.Service;
using Beavask.Application.Interface;
using Beavask.Domain.Entities.Base;
using Beavask.Application.Helper;
using Beavask.Application.DTOs.User;

namespace Beavask.Application.Service;

public class CompanyService(IUnitOfWork unitOfWork, IMapper mapper, IRepoService repoService) : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IRepoService _repoService = repoService;


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

}

