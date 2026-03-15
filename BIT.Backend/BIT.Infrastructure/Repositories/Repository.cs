using BIT.Domain.Entities;
using BIT.Domain.Interfaces;
using BIT.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BIT.Infrastructure.Repositories;

public class Repository<TKey, TEntity>(AppDbContext context) : IRepository<TKey, TEntity> where TEntity : Entity<TKey>
{
    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        if (predicate == null)
        {
            return await _dbSet.CountAsync(cancellationToken);
        }

        return await _dbSet.CountAsync(predicate, cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        TEntity trackedEntity = await _dbSet.FindAsync([entity.Id, cancellationToken], cancellationToken: cancellationToken) ?? throw new InvalidOperationException("Entity not found");
        AttachIfNotTracked(trackedEntity);
        _dbSet.Remove(trackedEntity);
    }

    public async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        foreach (var entity in entities)
        {
            TEntity trackedEntity = await _dbSet.FindAsync([entity.Id, cancellationToken], cancellationToken: cancellationToken) ?? throw new InvalidOperationException("Entity not found");
            AttachIfNotTracked(trackedEntity);
            _dbSet.Remove(trackedEntity);
        }
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken)
    {
        return await _dbSet.FindAsync([id], cancellationToken: cancellationToken);
    }

    public async Task<TEntity?> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbSet.SingleOrDefaultAsync(predicate, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetWithDetailsAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        TEntity trackedEntity = await _dbSet.FindAsync([entity.Id, cancellationToken], cancellationToken: cancellationToken) ?? throw new InvalidOperationException("Entity not found");

        // Detach if already tracked
        DetachIfTracked(trackedEntity);

        // Attach with modified state
        trackedEntity = entity;
        trackedEntity.UpdatedDate = DateTime.UtcNow;
        AttachIfNotTracked(trackedEntity);
        context.Entry(trackedEntity).State = EntityState.Modified;

        return trackedEntity;
    }

    private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
    {
        return SpecificationEvaluator<TKey, TEntity>.GetQuery(_dbSet.AsQueryable(), specification);
    }

    protected void AttachIfNotTracked(TEntity entity)
    {
        var entry = context.ChangeTracker.Entries<TEntity>().FirstOrDefault(e => e.Entity == entity);
        if (entry != null)
        {
            return;
        }

        _dbSet.Attach(entity);
    }

    protected void DetachIfTracked(TEntity entity)
    {
        var entry = context.ChangeTracker.Entries<TEntity>().FirstOrDefault(e => e.Entity == entity);
        entry?.State = EntityState.Detached;
    }

    public async Task<int> SaveAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}
