using System.ComponentModel.DataAnnotations.Schema;

namespace Beavask.Domain.Entities.Base
{
    public class Log
    {
        public int Id { get; set; }
        public required string ActivityType { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // User
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        // Company 
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company? Company { get; set; }
    }
}