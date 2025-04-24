using System;
using Beavask.Domain.Entities.Base;

namespace Beavask.Domain.Entities.Join
{
    public class TeamEvent
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        public Team Team { get; set; }  = null!;
        public int TeamId { get; set; }

        public Event Event { get; set; }  = null!;
        public int EventId { get; set; }

    }
}