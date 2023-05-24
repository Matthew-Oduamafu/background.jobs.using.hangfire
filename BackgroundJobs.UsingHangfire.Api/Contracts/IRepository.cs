using System.Linq.Expressions;

namespace BackgroundJobs.UsingHangfire.Api.Contracts;

public interface IRepository<TEntity>: IDisposable  where TEntity : class
{
    Task<IReadOnlyList<TEntity>> GetAll();
    Task<TEntity?> Get(Expression<Func<TEntity, bool>> predicate);
    Task<int> Add(TEntity entity, string propertyName);
    Task<int> Update(TEntity entity, string propertyName);
    Task Delete(Expression<Func<TEntity, bool>> predicate);
}