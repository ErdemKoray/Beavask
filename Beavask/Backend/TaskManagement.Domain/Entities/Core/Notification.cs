using System;

namespace TaskManagement.Domain.Entities.Core
{
    public class Notification
    {
        public int Id { get; set; }
        public string NotificationType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // User - Notification one-to-many relationship
        public required User User { get; set; }
        public int UserId { get; set; }
    }
}