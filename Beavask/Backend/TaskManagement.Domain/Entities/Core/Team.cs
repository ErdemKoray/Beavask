using System;  
using System.Collections.Generic;
using TaskManagement.Domain.Entities.Relationships;

namespace TaskManagement.Domain.Entities.Core
{
    public class Team
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // User - Team one-to-many relationship 
        public ICollection<User> TeamMembers = new List<User>();

        // Team - Event many-to-many relationship
        public ICollection<TeamEvent> Events = new List<TeamEvent>(); 

    }
}