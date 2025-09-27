using System.Linq.Expressions;

namespace ESourcing.Core.Repositories.Base;

public interface IRepository<T> where T : class, new()
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate);

    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate,
        Func<IQueryable<T>?, IOrderedQueryable<T>>? orderBy, string? includeString, bool disableTracking = true);

    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate,
        Func<IQueryable<T>?, IOrderedQueryable<T>>? orderBy, List<Expression<Func<T, object>>>? includes,
        bool disableTracking = true);

    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}