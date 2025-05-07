using Beavask.Application.DTOs.Task;
using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Infrastructure.Persistence;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class TaskRepository : BaseRepository<Beavask.Domain.Entities.Base.Task, int>, ITaskRepository
{
    private readonly BeavaskDbContext _context;
    public TaskRepository(BeavaskDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> IsUserAssignedToTask(int taskId, int userId)
    {
        var isAssigned = await _context.Tasks
            .AnyAsync(t => t.Id == taskId && t.AssignedUserId == userId && t.IsActive == true); 

        return isAssigned;
    }

    public async Task<bool> IsTaskTitleExistsAsync(string title, int projectId)
    {
        var taskExists = await _context.Tasks
            .Where(t => t.Title == title && t.ProjectId == projectId && t.IsActive == true)
            .AnyAsync();

        return taskExists;
    }

    public async Task<IEnumerable<TaskDto>> GetAllByProjectIdAsync(int projectId)
    {
        var tasks = await _context.Tasks
            .Where(t => t.ProjectId == projectId && t.IsActive == true)
            .Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                StartDate = t.StartDate,
                DueDate = t.DueDate,
                CompletedDate = t.CompletedDate,
                Priority = t.Priority,
                Status = t.Status,
                ProjectId = t.ProjectId,
                AssignedUserId = t.AssignedUserId
            })
            .ToListAsync();
            return tasks;
    }
} 