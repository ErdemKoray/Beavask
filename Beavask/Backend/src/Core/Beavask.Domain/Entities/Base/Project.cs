using System;
using System.Collections.Generic;
using Beavask.Domain.Entities.Join;

namespace Beavask.Domain.Entities.Base
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
        public ICollection<Milestone> Milestones = new List<Milestone>();
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<Task> Tasks = new List<Task>();
    }
}
