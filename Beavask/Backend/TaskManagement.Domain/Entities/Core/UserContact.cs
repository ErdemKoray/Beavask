using System;

namespace TaskManagement.Domain.Entities.Core
{
    public class UserContact
    {
        public int Id { get; set; }
        public string ContactType { get; set; } = string.Empty; 
        public string ContactValue { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // User - Contact one-to-many relationship
        public int UserId { get; set; }
        public required User User { get; set; }
    }
}
