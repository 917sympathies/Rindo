using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rindo.Domain.Repositories;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public abstract class RepositoryBase<T> where T : class
{
    private readonly RindoDbContext _context;
    
    protected RepositoryBase(RindoDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(IEnumerable<T> entities)
    {
        await _context.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task CreateAsync(T entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public Task Delete(T entity)
    {
        _context.Remove(entity);
        return Task.CompletedTask;
    }

    public Task Update(T entity)
    {
        _context.Update(entity);
        return Task.CompletedTask;
    } 
        

    public Task UpdatePropertyAsync<TProperty>(T entity, Expression<Func<T, TProperty>> expression) => Task.Run(() =>
        _context.Entry(entity).Property(expression).IsModified = true);

    public bool UpdateCollectionAsync<TProperty>(T entity, Expression<Func<T,IEnumerable<TProperty>>> expression) where TProperty : class
     => _context.Entry(entity).Collection(expression).IsModified = true;

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
}