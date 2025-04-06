using System;
using System.Collections.Generic;
using TaskManagement.Domain.Entities.Relationships;

namespace TaskManagement.Domain.Entities.Core
{
    public class Role
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public ICollection<RolePermission> Permissons { get; set; } = new List<RolePermission>();
    }
}