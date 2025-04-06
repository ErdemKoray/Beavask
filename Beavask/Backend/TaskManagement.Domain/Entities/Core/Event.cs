using System;
using TaskManagement.Domain.Entities.Relationships;

namespace TaskManagement.Domain.Entities.Core
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime EventDate { get; set; }
        public bool IsActive { get; set; } = true;

        // Team - Event many-to-many relationship
        public ICollection<TeamEvent> Teams { get; set; } = new List<TeamEvent>();
    }
}