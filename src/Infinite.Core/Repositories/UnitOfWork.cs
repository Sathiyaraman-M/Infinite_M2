﻿using System.Collections;
using Infinite.Core.Persistence;

namespace Infinite.Core.Repositories;

public class UnitOfWork(AppDbContext appDbContext) : IUnitOfWork
{
    public AppDbContext AppDbContext { get; } = appDbContext;
    private bool _disposed;
    private Hashtable _repositories;

    public IRepositoryAsync<T> GetRepository<T>() where T : class, IEntity
    {
        _repositories ??= new Hashtable();

        var type = typeof(T).Name;

        if (_repositories.ContainsKey(type)) return (IRepositoryAsync<T>)_repositories[type];
        
        var repositoryType = typeof(RepositoryAsync<>);
        var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), AppDbContext);
        _repositories.Add(type, repositoryInstance);

        return (IRepositoryAsync<T>)_repositories[type];
    }

    public async Task<int> Commit()
    {
        return await AppDbContext.SaveChangesAsync();
    }

    public Task Rollback()
    {
        AppDbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                //dispose managed resources
                AppDbContext.Dispose();
            }
        }
        //dispose unmanaged resources
        _disposed = true;
    }
}
