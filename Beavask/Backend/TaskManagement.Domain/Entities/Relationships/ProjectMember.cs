using System;
using TaskManagement.Domain.Entities.Core;

namespace TaskManagement.Domain.Entities.Relationships
{
    public class ProjectMember
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } 
        public bool IsActive { get; set; } = true;

        public int UserId { get; set; }
        public required User User { get; set; }

        public int ProjectId { get; set; }
        public required Project Project { get; set; }
    }
}



