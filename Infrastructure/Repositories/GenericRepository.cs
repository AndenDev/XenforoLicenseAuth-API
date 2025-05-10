using System.Linq.Expressions;
using Application.Interfaces;
using Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly XenforoDbContext _context;
        private readonly IMemoryCache _cache;

        public GenericRepository(XenforoDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(
     Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
     Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
     CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{typeof(TEntity).FullName}_GetAll";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<TEntity> cachedList))
            {
                return cachedList;
            }

            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var result = await query.ToListAsync(cancellationToken);

            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));

            return result;
        }

        public async Task<TEntity?> GetAsync(
     Expression<Func<TEntity, bool>>? predicate = null,
     Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
     Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
     CancellationToken cancellationToken = default)
        {
            var keyBase = $"{typeof(TEntity).FullName}_Get";
            var predicateKey = predicate != null ? predicate.ToString() : "NoPredicate";
            var cacheKey = $"{keyBase}_{predicateKey}";

            if (_cache.TryGetValue(cacheKey, out TEntity cachedEntity))
            {
                return cachedEntity;
            }

            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            if (result != null)
            {
                _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));
            }

            return result;
        }

        public async Task<TEntity?> FindByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().FindAsync(new[] { id }, cancellationToken);
        }

        public IQueryable<TEntity> Queryable()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public async Task<bool> ExistsAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate, cancellationToken);
        }
    }
}
