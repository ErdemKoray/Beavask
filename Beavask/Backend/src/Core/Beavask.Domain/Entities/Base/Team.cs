using System;  
using System.Collections.Generic;
using Beavask.Domain.Entities.Join;

namespace Beavask.Domain.Entities.Base
{
    public class Team
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // User - Team one-to-many relationship 
        public ICollection<User> TeamMembers { get; set; } = new List<User>();

        // Team - Event many-to-many relationship
        public ICollection<TeamEvent> Events { get; set; } = new List<TeamEvent>(); 

    }
}