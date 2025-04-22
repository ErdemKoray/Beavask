namespace Beavask.Application.DTOs.NotificationDtos;

public class NotificationCreateDto
{
    public string NotificationType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int UserId { get; set; }
}
