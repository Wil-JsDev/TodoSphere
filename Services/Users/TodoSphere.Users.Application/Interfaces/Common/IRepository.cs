using System.Linq.Expressions;

namespace TodoSphere.Users.Application.Interfaces.Common;

/// <summary>
/// Defines a generic repository interface providing basic CRUD operations for a specified entity type.
/// </summary>
/// <typeparam name="TEntity">The type of the entity the repository will manage. Must be a reference type.</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<bool> ValidateAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    void Update(TEntity entity);

    void Remove(TEntity entity);
}