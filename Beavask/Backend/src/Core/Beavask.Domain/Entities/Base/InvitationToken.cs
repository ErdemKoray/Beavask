using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beavask.Domain.Entities.Base
{
    [Table("InvitationTokens")]
    public class InvitationToken
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Token { get; set; }

        [Required]
        [MaxLength(150)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string? GitHubUsername { get; set; }  

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public bool IsUsed { get; set; } = false;

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }

        public int? CreatedByUserId { get; set; }
    }
}
