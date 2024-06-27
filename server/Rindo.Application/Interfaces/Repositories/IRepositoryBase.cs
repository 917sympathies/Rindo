using System.Linq.Expressions;

namespace Rindo.Domain.Repositories;

public interface  IRepositoryBase <T>
{
    Task CreateAsync(IEnumerable<T> entities);
    Task CreateAsync(T entity);
    Task DeleteAsync(T entity);
    Task UpdateAsync(T entity);
    IQueryable<T> FindAll();
    Task UpdatePropertyAsync<TProperty>(T entity, Expression<Func<T, TProperty>> expression);

    Task UpdateCollectionAsync<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> expression)
        where TProperty : class;
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    IQueryable<T> FindAll(bool trackChanges);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
}