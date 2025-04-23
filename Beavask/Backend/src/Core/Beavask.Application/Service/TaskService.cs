using AutoMapper;
using Beavask.Application.Common;
using Beavask.Application.DTOs.Task;
using Beavask.Application.Interface.Service;
using Beavask.Application.Interface;


namespace Beavask.Application.Service;

public class TaskService(IUnitOfWork unitOfWork, IMapper mapper) : ITaskService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

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
            var entity = _mapper.Map<Domain.Entities.Base.Task>(dto);
            await _unitOfWork.TaskRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var taskDto = _mapper.Map<TaskDto>(entity);
            return Response<TaskDto>.Success(taskDto);
        }
        catch (Exception ex)
        {
            return Response<TaskDto>.Fail(ex.Message);
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
}

