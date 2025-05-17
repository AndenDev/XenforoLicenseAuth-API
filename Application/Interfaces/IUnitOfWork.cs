
using Application.Repository;

namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Provides access to standard CRUD operations.
        /// </summary>
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;

        /// <summary>
        /// Provides access to paginated and filtered queries.
        /// </summary>
        IPaginatedGenericRepository<TEntity> PagedRepository<TEntity>() where TEntity : class;

        /// <summary>
        /// Saves all pending changes.
        /// </summary>
        Task<bool> SaveAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Begins a new transaction.
        /// </summary>
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Commits the current transaction.
        /// </summary>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rolls back the current transaction.
        /// </summary>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
