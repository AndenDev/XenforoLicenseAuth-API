using Application.Interfaces;
using Infrastructure.Configuration;
using Microsoft.Extensions.Caching.Memory;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly XenforoDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public UnitOfWork(XenforoDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
            => new GenericRepository<TEntity>(_context, _memoryCache);

        public async Task<bool> SaveAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
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

        public void Dispose() => _context.Dispose();
    }
}
