using System;
using System.Collections.Generic;
using Beavask.Domain.Entities.Join;

namespace Beavask.Domain.Entities.Base
{
    public class Role
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public ICollection<RolePermission> Permissons { get; set; } = new List<RolePermission>();
    }
}