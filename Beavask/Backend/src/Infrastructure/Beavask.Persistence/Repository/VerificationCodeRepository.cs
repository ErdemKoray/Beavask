using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class VerificationCodeRepository : BaseRepository<VerificationCode, int>, IVerificationCodeRepository
{
    public VerificationCodeRepository(DbContext context) : base(context)
    {
    }
}
