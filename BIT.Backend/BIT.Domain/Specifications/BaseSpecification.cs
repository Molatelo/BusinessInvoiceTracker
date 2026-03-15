using BIT.Domain.Interfaces;
using System.Linq.Expressions;

namespace BIT.Domain.Specifications;

public class BaseSpecification<TEntity> : ISpecification<TEntity>
{
    public BaseSpecification()
    {
    }

    public BaseSpecification(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<TEntity, bool>> Criteria { get; private set; } = _ => true;
    public List<Expression<Func<TEntity, object>>> Includes { get; } = [];
    public Expression<Func<TEntity, object>> OrderBy { get; private set; } = _ => true;
    public Expression<Func<TEntity, object>> OrderByDescending { get; private set; } = _ => false;
    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPagingEnabled { get; private set; }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

    protected void ApplyOrderBy(Expression<Func<TEntity, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void ApplyOrderByDescending(Expression<Func<TEntity, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }
}
