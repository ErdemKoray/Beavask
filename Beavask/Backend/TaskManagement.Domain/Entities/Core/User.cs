using System;
using System.Collections.Generic;
using TaskManagement.Domain.Entities.Relationships;

namespace TaskManagement.Domain.Entities.Core
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        // User - Project many-to-many relationship
        public ICollection<ProjectMember> Projects {get; set;} = new List<ProjectMember>();

        // User - UserContact one-to-many relationship
        public ICollection<UserContact> Contacts { get; set; } = new List<UserContact>();

        // User - Message one-to-many relationship
        public ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();

        // User - Role many-to-many relationship
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        // User - Team one-to-many relationship 
        public int TeamId { get; set; }
        public required Team Team { get; set; }
 
        // User - Company one-to-many relationship
        public int CompanyId { get; set; }
        public required Company Company { get; set; }

        // User - Problem one-to-many relationship
        public ICollection<Problem> Problems = new List<Problem>();

        // User - Log one-to-many relationship
        public ICollection<Log> Logs = new List<Log>();
        
        // User - Notification one-to-many relationship
        public ICollection<Notification> Notifications = new List<Notification>();
    }
}