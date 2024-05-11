using System.Linq.Expressions;
using Infinite.Core.Persistence;

namespace Infinite.Core.Repositories;

public class RepositoryAsync<T>(AppDbContext appDbContext) : IRepositoryAsync<T> where T : class, IEntity
{
    public IQueryable<T> Entities => appDbContext.Set<T>();

    public async Task<T> AddAsync(T entity)
    {
        await appDbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        return await appDbContext.Set<T>().CountAsync(predicate);
    }

    public async Task<int> CountAsync()
    {
        return await appDbContext.Set<T>().CountAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        appDbContext.Set<T>().Remove(entity);
        await Task.CompletedTask;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await appDbContext.Set<T>().ToListAsync();
    }

    public async Task<T> GetByIdAsync<TId>(TId id)
    {
        return await appDbContext.Set<T>().FindAsync(id);
    }

    public async Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize)
    {
        return await appDbContext.Set<T>().Skip((pageNumber - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
    }

    public async Task UpdateAsync<TId>(T updatedEntity, TId id)
    {
        T old = await appDbContext.Set<T>().FindAsync(id);
        appDbContext.Entry(old).CurrentValues.SetValues(updatedEntity);
        await Task.CompletedTask;
    }
}
