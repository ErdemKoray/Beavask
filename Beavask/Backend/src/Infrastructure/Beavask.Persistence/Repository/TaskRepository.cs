using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class TaskRepository : BaseRepository<Beavask.Domain.Entities.Base.Task, int>, ITaskRepository
{
    public TaskRepository(DbContext context) : base(context)
    {
    }
} 