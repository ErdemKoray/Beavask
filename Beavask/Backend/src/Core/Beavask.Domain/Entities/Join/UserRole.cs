using System;
using Beavask.Domain.Entities.Base;

namespace Beavask.Domain.Entities.Join
{
    public class UserRole
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public int UserId { get; set; }
        public required User User { get; set; } 

        public int RoleId { get; set; }
        public required Role Role { get; set; }
    }
}