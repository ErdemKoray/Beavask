using System;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities.Core
{
    public class Task
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        public bool IsActive { get; set; } = true;

        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public int? AssignedUserId { get; set; }
        public User AssignedUser { get; set; }

        public ICollection<TimeTracking> TimeTrackings = new List<TimeTracking>();
        
        public ICollection<Dependency> Dependencies = new List<Dependency>();

        public ICollection<Comment> Comments = new List<Comment>();
        
        public ICollection<File> Files = new List<File>();
    }
}
