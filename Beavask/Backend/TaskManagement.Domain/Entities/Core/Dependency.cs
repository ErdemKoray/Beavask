using System;

namespace TaskManagement.Domain.Entities.Core
{
    public class Dependency
    {
        public int Id { get; set; }

        public int DependentTaskId { get; set; }
        public Task DependentTask { get; set; }

        public int DependencyTaskId { get; set; } 
        public Task DependencyTask { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public string? Description { get; set; }
    }
}
