using Beavask.Domain.Enums;

namespace Beavask.Application.DTOs.Task;

public class TaskUpdateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public TaskPriority Priority { get; set; }
    public Domain.Enums.TaskStatus Status { get; set; }
    public int? AssignedUserId { get; set; }
}

