using System.Collections.Generic;

namespace CIT_Portfolio_Project_API.Application.DTOs;

/// <summary>
/// Generic paging envelope with HATEOAS links and items collection.
/// </summary>
public class PageDto<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public long Total { get; set; }
    public List<LinkDto> Links { get; set; } = new();
    public IEnumerable<T> Items { get; set; } = Array.Empty<T>();
}
