using System;
using System.Collections.Generic;
using Beavask.Domain.Entities.Join;

namespace Beavask.Domain.Entities.Base
{
    public class Project
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string RepoUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsCompanyProject { get; set; } = false;

        // For Personal Projects
        public int? UserId { get; set; } = null;
        public User? User { get; set; } = null;

        // For Company Projects
        public int? CompanyId { get; set; } = null;
        public Company? Company { get; set; }
        public int? CustomerId { get; set; } = null;
        public Customer? Customer { get; set; }

        public ICollection<ProjectMember> Members = new List<ProjectMember>();
        public ICollection<Milestone> Milestones = new List<Milestone>();
        public ICollection<Task> Tasks = new List<Task>();
    }

}
