using System.Linq.Expressions;
using ESourcing.Order.Domain.Entities.Base;
using ESourcing.Order.Domain.Repositories.Base;
using ESourcing.Order.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ESourcing.Order.Infrastructure.Repositories.Base;

public class Repository<T>(OrderContext context) : IRepository<T>
    where T : Entity
{
    public async Task<T> AddAsync(T entity)
    {
        context.Set<T>().Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await context.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T?>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await context.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T?>> GetAsync(Expression<Func<T, bool>>? predicate,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, string? includeString = null, bool disableTracking = true)
    {
        ArgumentNullException.ThrowIfNull(orderBy);
        IQueryable<T> query = context.Set<T>();
        if (disableTracking) query = query.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);
        if (predicate != null) query = query.Where(predicate);
        return await orderBy(query).ToListAsync();
    }
}