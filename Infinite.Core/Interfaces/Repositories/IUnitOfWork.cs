using Infinite.Core.Persistence;

namespace Infinite.Core.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    AppDbContext AppDbContext { get; }
    IRepositoryAsync<T> GetRepository<T>() where T : class, IEntity;
    Task<int> Commit();
    Task Rollback();
}
