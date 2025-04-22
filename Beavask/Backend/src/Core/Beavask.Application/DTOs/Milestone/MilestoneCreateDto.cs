using System;

namespace Beavask.Application.DTOs.Milestone
{
    public class MilestoneCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; } = false;
        
        public int ProjectId { get; set; } 
    }
}
