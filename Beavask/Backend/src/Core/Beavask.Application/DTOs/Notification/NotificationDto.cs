using System;

namespace Beavask.Application.DTOs.NotificationDtos;

public class NotificationDto
{
    public int Id { get; set; }
    public string NotificationType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
}
