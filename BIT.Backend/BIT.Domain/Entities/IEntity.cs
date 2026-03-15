namespace BIT.Domain.Entities;

public interface IEntity : IEntity<int>
{
}

public interface IEntity<TKey>
{
    TKey Id { get; }

    bool IsTransient();
}
