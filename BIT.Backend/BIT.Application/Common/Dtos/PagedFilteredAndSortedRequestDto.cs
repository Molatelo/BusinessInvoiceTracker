using System.ComponentModel.DataAnnotations;

namespace BIT.Application.Common.Dtos;

public class PagedFilteredAndSortedRequestDto
{
    public int PageNumber { get; set; }

    [Range(1, 1000)]
    public int PageSize { get; set; }
    public string? SortBy { get; set; }
    public bool IsDescending { get; set; }
    public string? Filter { get; set; }
}
