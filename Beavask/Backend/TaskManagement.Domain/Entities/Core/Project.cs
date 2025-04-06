using System;
using System.Collections.Generic;
using TaskManagement.Domain.Entities.Relationships;

namespace TaskManagement.Domain.Entities.Core
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }  = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<ProjectMember> Members = new List<ProjectMember>();        
    }
}
