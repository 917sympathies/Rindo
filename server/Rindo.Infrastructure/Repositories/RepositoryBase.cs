using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public abstract class RepositoryBase<T>(PostgresDbContext context) where T : class
{
    protected async Task CreateAsync(IEnumerable<T> entities)
    {
        await context.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    protected async Task<T> CreateAsync(T entity)
    {
        var createdEntity = await context.AddAsync(entity);
        await context.SaveChangesAsync();
        return createdEntity.Entity;
    }

    protected void Delete(T entity)
    {
        context.Remove(entity);
        context.SaveChanges();
    }
    
    protected void DeleteMany(IEnumerable<T> entities)
    {
        context.RemoveRange(entities);
        context.SaveChanges();
    }

    protected void Update(T entity)
    {
        context.Update(entity);
        context.SaveChanges();
    }

    protected async Task UpdatePropertyAsync<TProperty>(T entity, Expression<Func<T, TProperty>> expression)
    {
        context.Entry(entity).Property(expression).IsModified = true;
        await context.SaveChangesAsync();
    }

    protected bool UpdateCollectionAsync<TProperty>(T entity, Expression<Func<T,IEnumerable<TProperty>>> expression) where TProperty : class
     => context.Entry(entity).Collection(expression).IsModified = true;

    protected IQueryable<T> FindAll() =>
        context.Set<T>().AsNoTracking();

    protected IQueryable<T> FindByCondition(Expression<Func<T,bool>> expression) =>
        context.Set<T>().Where(expression).AsNoTracking();

    protected IQueryable<T> FindAll(bool trackChanges) => !trackChanges
        ? context.Set<T>().AsNoTracking()
        : context.Set<T>();

    protected IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) => !trackChanges
        ? context.Set<T>().Where(expression).AsNoTracking()
        : context.Set<T>().Where(expression);
}