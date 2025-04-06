using System;
using TaskManagement.Domain.Entities.Core;

namespace TaskManagement.Domain.Entities.Relationships
{
    public class TeamEvent
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        public required Team Team { get; set; }
        public int TeamId { get; set; }

        public required Event Event { get; set; }
        public int EventId { get; set; }

    }
}