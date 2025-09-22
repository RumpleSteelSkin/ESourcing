using System.Linq.Expressions;
using ESourcing.Order.Domain.Entities.Base;

namespace ESourcing.Order.Domain.Repositories.Base;

public interface IRepository<T> where T : Entity
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T?>> GetAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T?>> GetAsync(Expression<Func<T, bool>>? predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string includeString = null, bool disableTracking = true);
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}