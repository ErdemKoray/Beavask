using AutoMapper;
using Beavask.Application.Common;
using Beavask.Application.DTOs.Task;
using Beavask.Application.Interface.Service;
using Beavask.Application.Interface;

namespace Beavask.Application.Service;

public class TaskService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService) : ITaskService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<Response<TaskDto>> GetByIdAsync(int id)
    {
        try
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id);
            if (task == null)
                return Response<TaskDto>.NotFound();

            var dto = _mapper.Map<TaskDto>(task);
            return Response<TaskDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<TaskDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<TaskDto>>> GetAllAsync()
    {
        try
        {
            var tasks = await _unitOfWork.TaskRepository.GetAsync();
            var dtos = _mapper.Map<IEnumerable<TaskDto>>(tasks);
            return Response<IEnumerable<TaskDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<TaskDto>>.Fail(ex.Message);
        }
    }

    public async Task<Response<TaskDto>> CreateAsync(TaskCreateDto dto)
    {
        try
        {
            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(dto.ProjectId);
            
            if (project == null)
            {
                return Response<TaskDto>.NotFound($"Project with ID {dto.ProjectId} not found.");
            }

            bool isTitleExists = await _unitOfWork.TaskRepository.IsTaskTitleExistsAsync(dto.Title, dto.ProjectId);
            if (isTitleExists)
            {
                return Response<TaskDto>.Fail($"A task with the title '{dto.Title}' already exists for this project.");
            }

            var task = _mapper.Map<Domain.Entities.Base.Task>(dto);
            task.ProjectId = dto.ProjectId;
            task.CreatorId = _currentUserService.UserId;

            await _unitOfWork.TaskRepository.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();

            var taskDto = _mapper.Map<TaskDto>(task); 
            
            return Response<TaskDto>.Success(taskDto);
        }
        catch (Exception ex)
        {
            return Response<TaskDto>.Fail($"An error occurred while creating the task: {ex.Message}");
        }
    }

    public async Task<Response<TaskDto>> UpdateAsync(int id, TaskUpdateDto dto)
    {
        try
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id);
            if (task == null)
                return Response<TaskDto>.NotFound();

            _mapper.Map(dto, task);
            await _unitOfWork.TaskRepository.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();

            var updatedDto = _mapper.Map<TaskDto>(task);
            return Response<TaskDto>.Success(updatedDto);
        }
        catch (Exception ex)
        {
            return Response<TaskDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        try
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(id);
            if (task == null)
                return Response<bool>.NotFound();

            await _unitOfWork.TaskRepository.DeleteAsync(task);
            await _unitOfWork.SaveChangesAsync();

            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail(ex.Message);
        }
    }
    public async Task<Response<bool>> AssigneToUserAsync(int taskId, int userId)
    {
        try
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(taskId);
            if (task == null)
            {
                return Response<bool>.NotFound("Task not found.");
            }

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return Response<bool>.NotFound("User not found.");
            }

            bool isAssigned = await _unitOfWork.TaskRepository.IsUserAssignedToTask(taskId, userId);
            if (isAssigned)
            {
                return Response<bool>.Fail("User is already assigned to this task.");
            }

            task.AssignedUserId = user.Id;

            await _unitOfWork.TaskRepository.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();

            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail($"Error occurred: {ex.Message}");
        }
    }

    public async Task<Response<IEnumerable<TaskDto>>> GetAllTaskByProjectIdAsync(int projectId)
    {
        try
        {
            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(projectId);
            if (project == null)
            {
                return Response<IEnumerable<TaskDto>>.NotFound($"Project with ID {projectId} not found.");
            }
            var tasks = await _unitOfWork.TaskRepository.GetAllByProjectIdAsync(projectId);

            if (tasks == null || !tasks.Any())
            {
                return Response<IEnumerable<TaskDto>>.NotFound($"No tasks found for project with ID {projectId}.");
            }
            var taskDtos = _mapper.Map<IEnumerable<TaskDto>>(tasks);
            return Response<IEnumerable<TaskDto>>.Success(taskDtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<TaskDto>>.Fail($"Error occurred: {ex.Message}");
        }
    }
}

