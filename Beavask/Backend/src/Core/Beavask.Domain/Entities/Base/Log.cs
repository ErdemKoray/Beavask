using System;

namespace Beavask.Domain.Entities.Base
{
    public class Log
    {
        public int Id { get; set; }
        public required string ActivityType { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // User - Log one-to-many relationship
        public required User User { get; set;}
        public int UserId { get; set; }
    }
}