using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface ITaskRepository : IBaseRepository<Beavask.Domain.Entities.Base.Task, int>
{
    Task<bool> IsUserAssignedToTask(int taskId, int userId);
    Task<bool> IsTaskTitleExistsAsync(string title, int projectId);
    Task<IEnumerable<Domain.Entities.Base.Task>> GetAllByProjectIdAsync(int projectId);
} 