using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TodoSphere.Auth.Application.Interfaces.Repositories;
using TodoSphere.Auth.Persistence.Context;

namespace TodoSphere.Auth.Persistence.Repository;

public class Repository<TEntity>(AuthContext context) : IRepository<TEntity>
    where TEntity : class
{
    protected readonly AuthContext Context = context;
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public virtual async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return (await _dbSet.FindAsync([id], cancellationToken).AsTask())!;
    }

    public virtual async Task<bool> ValidateAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken);
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public virtual void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void Add(TEntity entity)
    {
        _dbSet.Add(entity); 
    }

    public virtual void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
}