using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface INotificationRepository : IBaseRepository<Notification, int>
{
    // Add any notification-specific repository methods here
} 