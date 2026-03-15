namespace BIT.Domain.Entities;

[Serializable]
public abstract class Entity : Entity<int>, IEntity
{

}

[Serializable]
public abstract class Entity<TKey> : IEntity<TKey>
{
    public virtual TKey Id { get; protected set; } = default!;

    public virtual DateTime CreatedDate { get; private set; }

    public virtual DateTime? UpdatedDate { get; set; }

    protected Entity()
    {

    }

    protected Entity(TKey id)
    {
        Id = id;
    }

    public virtual bool IsTransient()
    {
        if (EqualityComparer<TKey>.Default.Equals(Id, default!))
        {
            return true;
        }

        if (typeof(TKey) == typeof(int))
        {
            return Convert.ToInt32(Id) <= 0;
        }

        if (typeof(TKey) == typeof(long))
        {
            return Convert.ToInt64(Id) <= 0L;
        }

        return false;
    }

    public override string ToString()
    {
        return $"[{GetType().Name} {Id}]";
    }
}
