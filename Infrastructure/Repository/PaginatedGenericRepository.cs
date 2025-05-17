using Application.DTOs.Request;
using Application.Models;
using Application.Repository;
using Application.Utilities.Extensions;
using Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository
{
    public class PaginatedGenericRepository<TEntity> : IPaginatedGenericRepository<TEntity> where TEntity : class
    {
        private readonly XenforoDbContext _context;

        public PaginatedGenericRepository(XenforoDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<TEntity>> PagedResultAsync(
            BaseFilterDto filterDto,
            string defaultSortColumn,
            bool defaultSortDescending = false,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            if (include != null)
                query = include(query);

            query = query.ApplyDynamicFilters(filterDto);
            query = query.ApplyDynamicSorting(filterDto.SortOptions, defaultSortColumn, defaultSortDescending);

            int totalCount = await query.CountAsync(cancellationToken);
            int totalPages = (int)Math.Ceiling(totalCount / (double)filterDto.PageSize);
            int pageNumber = Math.Min(filterDto.PageNumber, Math.Max(totalPages, 1));
            int offset = Math.Max((pageNumber - 1) * filterDto.PageSize, 0);
            bool hasMore = pageNumber < totalPages;

            var items = await query.Skip(offset).Take(filterDto.PageSize).ToListAsync(cancellationToken);

            return new PagedResult<TEntity>
            {
                Items = items,
                TotalPages = totalPages,
                PageSize = filterDto.PageSize,
                PageNumber = pageNumber,
                HasMore = hasMore,
                TotalItemCount = totalCount
            };
        }
    }
}
