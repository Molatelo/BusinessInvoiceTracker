using BIT.Domain.Entities;
using System.Linq.Expressions;

namespace BIT.Domain.Interfaces;

public interface IRepository<TKey, TEntity> where TEntity : Entity<TKey>
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken);
    Task<TEntity?> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
    Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetWithDetailsAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken);
    Task<int> SaveAsync(CancellationToken cancellationToken);
}
