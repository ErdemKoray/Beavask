using AutoMapper;
using Beavask.Application.Common;
using Beavask.Application.DTOs.Project;
using Beavask.Application.Interface.Service;
using Beavask.Application.Interface;
using Beavask.Domain.Entities.Base;
using Beavask.Application.DTOs.Repo;

namespace Beavask.Application.Service;

public class ProjectService(IUnitOfWork unitOfWork, IMapper mapper) : IProjectService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentUserService? _currentUserService;

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

    public async Task<Response<ProjectDto>> CreateAsync(ProjectCreateDto projectCreateDto)
    {
        try
        {
            var entity = _mapper.Map<Project>(projectCreateDto);
            await _unitOfWork.ProjectRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<ProjectDto>(entity);
            return Response<ProjectDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<ProjectDto>.Fail(ex.Message);
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
    public async Task<Response<bool>> CreateProjectFromSingleGitHubRepoAsync(CreateProjectFromGitHubRepoDto repo, int? userId)
    {
        if (repo == null)
            return Response<bool>.Fail("Repo bilgisi boş.");

        if (string.IsNullOrWhiteSpace(repo.Name))
            return Response<bool>.Fail("Proje adı boş olamaz.");

        if (string.IsNullOrWhiteSpace(repo.RepoUrl))
            return Response<bool>.Fail("Repo URL boş olamaz.");

        if (!repo.IsCompanyProject && userId == null)
        {
            return Response<bool>.Fail("Bireysel proje oluşturuluyor ancak kullanıcı ID bulunamadı.");
        }

        var project = new Project
        {
            Name = repo.Name,
            Description = repo.Description ?? string.Empty,
            RepoUrl = repo.RepoUrl,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            OwnerId = repo.IsCompanyProject ? null : userId,
            CustomerId = null
        };

        await _unitOfWork.ProjectRepository.AddAsync(project);
        await _unitOfWork.SaveChangesAsync();

        return Response<bool>.Success(true);
    }

}

