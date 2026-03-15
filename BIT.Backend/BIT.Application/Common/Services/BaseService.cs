using BIT.Domain.Entities;
using BIT.Domain.Interfaces;
using MapsterMapper;

namespace BIT.Application.Common.Services;

public class BaseService<Tkey, TEntity, TEntityDto, TCreateEntityDto, TUpdateEntityDto>(IRepository<Tkey, TEntity> repository, IMapper mapper) where TEntity : Entity<Tkey>
{
    public virtual async Task<TEntityDto> CreateAsync(TCreateEntityDto createDto, CancellationToken cancellationToken = default)
    {
        var entity = mapper.Map<TEntity>(createDto!);

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveAsync(cancellationToken);

        return mapper.Map<TEntityDto>(entity);
    }

    public virtual async Task<TEntityDto?> GetByIdAsync(Tkey id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken);

        return mapper.Map<TEntityDto>(entity!);
    }

    public virtual async Task<IEnumerable<TEntityDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await repository.GetAllAsync(cancellationToken);

        return mapper.Map<IEnumerable<TEntityDto>>(entities);
    }

    public virtual async Task<TEntityDto> UpdateAsync(Tkey id, TUpdateEntityDto updateDto, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken) ?? throw new KeyNotFoundException($"Entity with id {id} not found.");
        mapper.Map(updateDto!, entity);

        entity = await repository.UpdateAsync(entity, cancellationToken);
        await repository.SaveAsync(cancellationToken);

        return mapper.Map<TEntityDto>(entity);
    }

    public virtual async Task DeleteAsync(Tkey id, CancellationToken cancellationToken = default)
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken) ?? throw new KeyNotFoundException($"Entity with id {id} not found.");
        await repository.DeleteAsync(entity, cancellationToken);
        await repository.SaveAsync(cancellationToken);
    }
}
