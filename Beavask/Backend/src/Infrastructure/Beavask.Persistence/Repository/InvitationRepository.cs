using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities;
using Beavask.Domain.Entities.Base;
using Beavask.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository
{
    public class InvitationTokenRepository : BaseRepository<InvitationToken, int>, IInvitationTokenRepository
    {
        private readonly BeavaskDbContext _context;

        public InvitationTokenRepository(BeavaskDbContext context) : base(context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task AddAsync(InvitationToken token)
        {
            await _context.InvitationTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task<InvitationToken?> GetByTokenAsync(string token)
        {
            return await _context.InvitationTokens
                .FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task<bool> IsTokenValidAsync(string token)
        {
            var invitation = await _context.InvitationTokens
                .FirstOrDefaultAsync(x => x.Token == token);

            if (invitation == null)
                return false;

            if (invitation.IsUsed)
                return false;

            if (invitation.ExpiresAt < DateTime.UtcNow)
                return false;

            return true;
        }

        public async System.Threading.Tasks.Task MarkAsUsedAsync(string token)
        {
            var invitation = await _context.InvitationTokens
                .FirstOrDefaultAsync(x => x.Token == token);

            if (invitation != null)
            {
                invitation.IsUsed = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
