using System;
using System.Collections.Generic;
using TaskManagement.Domain.Entities.Relationships;

namespace TaskManagement.Domain.Entities.Core
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<RolePermission> Roles { get; set; } = new List<RolePermission>();
    }
}