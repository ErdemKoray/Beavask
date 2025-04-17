using System;

namespace Beavask.Domain.Entities.Base
{
    public class TimeTracking
    {
        public int Id { get; set; }

        public int TaskId { get; set; }
        public Task Task { get; set; }
        
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        
        public TimeSpan? Duration { get; set; }
        
        public string? Notes { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
