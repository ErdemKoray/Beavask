using Beavask.Domain.Entities.Base;
using Beavask.Domain.Enums;

namespace Beavask.Domain.Entities.Join;

public class ProjectInvitation 
{
    public int Id { get; set; }
    public ProjectInvitationStatus Status { get; set; } = ProjectInvitationStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public User Sender { get; set; }
    public int SenderId { get; set; }
    public User Receiver { get; set; }
    public int ReceiverId { get; set; }
    public Project Project { get; set; }
    public int ProjectId { get; set; }
}
