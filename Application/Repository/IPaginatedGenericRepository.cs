
using System.Linq.Expressions;
using Application.DTOs.Request;
using Application.Models;

namespace Application.Repository
{
    public interface IPaginatedGenericRepository<TEntity> where TEntity : class
    {
        Task<PagedResult<TEntity>> PagedResultAsync(
            BaseFilterDto filterDto,
            string defaultSortColumn,
            bool defaultSortDescending = false,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            CancellationToken cancellationToken = default);
    }
}
