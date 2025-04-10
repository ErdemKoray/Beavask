using System;
using TaskManagement.Domain.Entities.Core;

namespace TaskManagement.Domain.Entities.Relationships
{
    public class RolePermission
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public required Role Role { get; set; }
        public int RoleId { get; set; }

        public required Permission Permisson { get; set; }
        public int PermissonId { get; set; }
    }
}