using System.Linq.Expressions;
using BackgroundJobs.UsingHangfire.Api.Contracts;
using BackgroundJobs.UsingHangfire.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace BackgroundJobs.UsingHangfire.Api.Services;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly HangfireDbContext _dbContext;

    public Repository(HangfireDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<TEntity>> GetAll()
    {
        return await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
    }

    public async Task<TEntity?> Get(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    public async Task<int> Add(TEntity entity, string propertyName)
    {
        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return _dbContext.Entry(entity).CurrentValues.GetValue<int>(propertyName);
    }

    public async Task<int> Update(TEntity entity, string propertyName)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        return _dbContext.Entry(entity).CurrentValues.GetValue<int>(propertyName);
    }

    public async Task Delete(Expression<Func<TEntity, bool>> predicate)
    {
        var entity = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        if (entity == null) return;
        
        _dbContext.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}