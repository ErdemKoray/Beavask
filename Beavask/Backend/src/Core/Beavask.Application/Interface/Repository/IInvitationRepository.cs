using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Interface.Repository;

public interface IInvitationTokenRepository : IBaseRepository<InvitationToken, int>
{
System.Threading.Tasks.Task AddAsync(InvitationToken token);
System.Threading.Tasks.Task<InvitationToken?> GetByTokenAsync(string token);
System.Threading.Tasks.Task<bool> IsTokenValidAsync(string token);
System.Threading.Tasks.Task MarkAsUsedAsync(string token);
}
