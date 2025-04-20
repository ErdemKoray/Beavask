using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class MessageRepository : BaseRepository<Message, int>, IMessageRepository
{
    public MessageRepository(DbContext context) : base(context)
    {
    }
} 