using System;

namespace Beavask.Domain.Entities.Base
{
    public class Problem
    {
        public int Id { get; set; }
        public string Title { get; set; }  = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        public required User User { get; set; }
        public int UserId { get; set; }
    } 
}