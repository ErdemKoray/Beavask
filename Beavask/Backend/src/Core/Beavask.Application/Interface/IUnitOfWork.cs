namespace Beavask.Application.Interfaces;

public interface IUnitOfWork
{

    
    Task<int> SaveChangesAsync();
}
