using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface ITaskRepository : IBaseRepository<Beavask.Domain.Entities.Base.Task, int>
{
    // Add any task-specific repository methods here
} 