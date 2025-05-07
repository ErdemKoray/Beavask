using Beavask.Application.Common;
using Beavask.Application.DTOs.Task;

namespace Beavask.Application.Interface.Service;
public interface ITaskService
{
    Task<Response<TaskDto>> GetByIdAsync(int id);
    Task<Response<IEnumerable<TaskDto>>> GetAllAsync();
    Task<Response<TaskDto>> CreateAsync(TaskCreateDto dto);
    Task<Response<TaskDto>> UpdateAsync(int id, TaskUpdateDto dto);
    Task<Response<bool>> DeleteAsync(int id);
    Task<Response<bool>> AssigneToUserAsync(int taskId, int userId);
}

