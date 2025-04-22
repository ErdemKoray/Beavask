namespace Beavask.Application.DTOs.NotificationDtos;

public class NotificationUpdateDto
{
    public string NotificationType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
