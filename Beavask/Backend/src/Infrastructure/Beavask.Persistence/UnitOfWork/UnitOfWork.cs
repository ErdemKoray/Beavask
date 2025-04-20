using Beavask.Application.Interfaces;
using Beavask.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly BeavaskDbContext _context;

    public UnitOfWork(BeavaskDbContext context)
    {
        _context = context;
    }


    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
