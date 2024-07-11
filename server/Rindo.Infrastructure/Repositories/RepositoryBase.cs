using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly RindoDbContext _context;
    protected RepositoryBase(RindoDbContext context)
    {
        _context = context;
    }
    
    public Task CreateAsync(IEnumerable<T> entities)
    {
        _context.AttachRange(entities);
        return Task.CompletedTask;
    }
    
    public Task CreateAsync(T entity)
    {
        _ = _context.Attach(entity);
        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(T entity) => Task.Run(() =>
        _context.Set<T>().Remove(entity));

    public Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public Task UpdatePropertyAsync<TProperty>(T entity, Expression<Func<T,TProperty>> expression)
    {
        _context.Entry(entity).Property(expression).IsModified = true;
        return Task.CompletedTask;
    }

    public Task UpdateCollectionAsync<TProperty>(T entity, Expression<Func<T,IEnumerable<TProperty>>> expression) where TProperty : class
    {
        _context.Entry(entity).Collection(expression).IsModified = true;
        return Task.CompletedTask;
    }

    public IQueryable<T> FindAll() =>
        _context.Set<T>().AsNoTracking();

    public IQueryable<T> FindByCondition(Expression<Func<T,bool>> expression) =>
        _context.Set<T>().Where(expression).AsNoTracking();

    public IQueryable<T> FindAll(bool trackChanges) => !trackChanges
        ? _context.Set<T>().AsNoTracking()
        : _context.Set<T>();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) => !trackChanges
        ? _context.Set<T>().Where(expression).AsNoTracking()
        : _context.Set<T>().Where(expression);

    public Task Save() => _context.SaveChangesAsync();
}