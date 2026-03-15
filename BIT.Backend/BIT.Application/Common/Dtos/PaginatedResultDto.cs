namespace BIT.Application.Common.Dtos;

public class PaginatedResultDto<TEntity>(int totalCount, IEnumerable<TEntity> items)
{
    public int TotalCount { get; set; } = totalCount;
    public IEnumerable<TEntity> Items { get; set; } = items;
}
