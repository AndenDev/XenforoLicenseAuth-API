using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Retrieves all entities, with optional includes and ordering.
        /// </summary>
        Task<IEnumerable<TEntity>> GetAllAsync(
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a single entity matching the predicate, with optional includes and ordering.
        /// </summary>
        Task<TEntity?> GetAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds an entity by its ID (primary key).
        /// </summary>
        Task<TEntity?> FindByIdAsync(
            object id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Provides queryable access to the entity set.
        /// </summary>
        IQueryable<TEntity> Queryable();

        /// <summary>
        /// Adds an entity to the database.
        /// </summary>
        Task AddAsync(
            TEntity entity,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an entity in the database.
        /// </summary>
        void Update(TEntity entity);

        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        void Delete(TEntity entity);

        /// <summary>
        /// Checks if any entities match the given predicate.
        /// </summary>
        Task<bool> ExistsAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);
    }
}
