using System;
using Beavask.Domain.Entities.Base;

namespace Beavask.Domain.Entities.Join
{
    public class RolePermission
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public required Role Role { get; set; }
        public int RoleId { get; set; }

        public required Permission Permission { get; set; }
        public int PermissionId { get; set; }
    }
}