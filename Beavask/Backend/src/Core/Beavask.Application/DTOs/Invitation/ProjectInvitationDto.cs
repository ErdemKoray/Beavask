using Beavask.Domain.Enums;

namespace Beavask.Application.DTOs.Invitation;

public class ProjectInvitationDto
{
    public int Id { get; set; }
    public ProjectInvitationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public int SenderId { get; set; }
    public string SenderName { get; set; }
    public int ProjectId { get; set; }
    public string ProjectName { get; set; }
}
