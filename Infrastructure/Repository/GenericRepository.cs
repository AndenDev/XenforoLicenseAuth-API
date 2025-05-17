
using Application.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Infrastructure.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly IMemoryCache _memoryCache;
        protected static ConcurrentBag<string> _cacheKeys = new();
        protected static readonly string _baseCacheKey = typeof(TEntity).FullName ?? typeof(TEntity).Name;

        public GenericRepository(DbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public IQueryable<TEntity> Queryable() => _context.Set<TEntity>().AsQueryable();

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            RemoveCacheKeys();
            await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            RemoveCacheKeys();
            await _context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        }

        public void Update(TEntity entity)
        {
            RemoveCacheKeys();
            _context.Set<TEntity>().Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            RemoveCacheKeys();
            _context.Set<TEntity>().UpdateRange(entities);
        }

        public void Delete(TEntity entity)
        {
            RemoveCacheKeys();
            _context.Set<TEntity>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            RemoveCacheKeys();
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public async Task<TEntity?> FindByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate, cancellationToken);
        }

        public async Task<TEntity?> GetAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (include != null)
                query = include(query);

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = orderBy(query);

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (include != null)
                query = include(query);

            if (orderBy != null)
                query = orderBy(query);

            return await query.ToListAsync(cancellationToken);
        }

        public void Detach(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public void DetachRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                Detach(entity);
        }

        protected void RemoveCacheKeys()
        {
            var keysToRemove = _cacheKeys.Where(k => k.Contains(_baseCacheKey)).ToList();
            foreach (var key in keysToRemove)
                _memoryCache.Remove(key);

            _cacheKeys = new ConcurrentBag<string>(_cacheKeys.Except(keysToRemove));
        }
    }
}
