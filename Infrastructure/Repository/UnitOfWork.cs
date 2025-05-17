using Application.Interfaces;
using Application.Repository;
using Infrastructure.Configuration;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace Infrastructure.Repository
{
    public sealed class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly XenforoDbContext _context;
        private readonly IMemoryCache _memoryCache;

        private readonly ConcurrentDictionary<Type, object> _genericRepositories = new();
        private readonly ConcurrentDictionary<Type, object> _pagedRepositories = new();

        public UnitOfWork(XenforoDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);

            if (!_genericRepositories.TryGetValue(type, out var repo))
            {
                repo = new GenericRepository<TEntity>(_context, _memoryCache);
                _genericRepositories.TryAdd(type, repo);
            }

            return (IGenericRepository<TEntity>)repo;
        }

        public IPaginatedGenericRepository<TEntity> PagedRepository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);

            if (!_pagedRepositories.TryGetValue(type, out var repo))
            {
                repo = new PaginatedGenericRepository<TEntity>(_context);
                _pagedRepositories.TryAdd(type, repo);
            }

            return (IPaginatedGenericRepository<TEntity>)repo;
        }

        public async Task<bool> SaveAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_context.Database.CurrentTransaction == null)
                await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_context.Database.CurrentTransaction != null)
                await _context.Database.CommitTransactionAsync(cancellationToken);
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_context.Database.CurrentTransaction != null)
                await _context.Database.RollbackTransactionAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
