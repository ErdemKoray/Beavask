using System.Linq.Expressions;
using Beavask.Application.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace Beavask.Persistence.Repository;

public class BaseRepository<T, TKey> : IBaseRepository<T, TKey> where T : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAsync(Func<IQueryable<T>, IQueryable<T>>? queryModifier = null)
    {
        var query = _dbSet.AsQueryable();
        if (queryModifier != null)
        {
            query = queryModifier(query);
        }
        return await query.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(TKey id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression, bool trackChanges)  => trackChanges ? _dbSet.Where(expression) :
                                                             _dbSet.Where(expression).AsNoTracking();
    
    public async Task<T?> GetSingleByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges = false)
    {
        var query = _dbSet.Where(expression);
        if (!trackChanges)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync();
    }
}
