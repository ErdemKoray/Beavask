using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class CompanyRepository : BaseRepository<Company, int>, ICompanyRepository
{
    public CompanyRepository(DbContext context) : base(context)
    {
    }
} 