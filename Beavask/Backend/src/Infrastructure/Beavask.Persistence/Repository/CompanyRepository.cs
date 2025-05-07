using Beavask.Application.Interface.Repository;
using Beavask.Domain.Entities.Base;
using Beavask.Infrastructure.Persistence;
using Beavask.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class CompanyRepository : BaseRepository<Company, int>, ICompanyRepository
{
    private readonly BeavaskDbContext _context;
    public CompanyRepository(BeavaskDbContext context) : base(context)
    {  
        _context = context;
    }
} 