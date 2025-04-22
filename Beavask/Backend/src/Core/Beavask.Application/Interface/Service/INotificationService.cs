using Beavask.Application.Common;
using Beavask.Application.DTOs.NotificationDtos;

public interface INotificationService
{
    Task<Response<IEnumerable<NotificationDto>>> GetAllAsync();
    Task<Response<NotificationDto>> CreateAsync(NotificationCreateDto dto);
}
