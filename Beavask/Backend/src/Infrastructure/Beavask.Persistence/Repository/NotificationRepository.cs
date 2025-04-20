using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class NotificationRepository : BaseRepository<Notification, int>, INotificationRepository
{
    public NotificationRepository(DbContext context) : base(context)
    {
    }
} 