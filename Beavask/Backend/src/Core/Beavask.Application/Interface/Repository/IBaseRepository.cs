using System.Linq.Expressions;

namespace Beavask.Application.Interface.Repository;

public interface IBaseRepository<T, TKey> where T : class
{
    Task<IEnumerable<T>> GetAsync(Func<IQueryable<T>, IQueryable<T>>? queryModifier = null);
    Task<T?> GetByIdAsync(TKey id);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
