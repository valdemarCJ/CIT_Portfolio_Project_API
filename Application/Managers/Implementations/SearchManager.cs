using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

namespace CIT_Portfolio_Project_API.Application.Managers.Implementations;

public class SearchManager : ISearchManager
{
    private readonly ISearchRepository _repo;
    public SearchManager(ISearchRepository repo) { _repo = repo; }

    /// <summary>
    /// Free-text search. Builds HATEOAS pagination links (self/prev/next) so clients can paginate.
    /// Note: this layer focuses on shaping results, not implementing the search algorithm.
    /// </summary>
    public async Task<PageDto<SearchDto>> StringSearchAsync(int userId, string text, int page, int pageSize, CancellationToken ct = default)
    {
        var rows = (await _repo.StringSearchAsync(userId, text, ct)).ToList();
        var total = rows.Count;
        var items = rows.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(r => new SearchDto { Tconst = r.Tconst, Title = r.Title, Links = new List<LinkDto> { new("self", $"/api/movies/{r.Tconst}") } })
            .ToList();
        var dto = new PageDto<SearchDto> { Page = page, PageSize = pageSize, Total = total, Items = items };
        AddPageLinks(dto, $"/api/search?query={Uri.EscapeDataString(text)}");
        return dto;
    }

    /// <summary>
    /// Structured search. Same pagination link approach as above, but with multiple filters.
    /// </summary>
    public async Task<PageDto<SearchDto>> StructuredSearchAsync(int userId, string? title, string? plot, string? characters, string? person, int page, int pageSize, CancellationToken ct = default)
    {
        var rows = (await _repo.StructuredSearchAsync(userId, title, plot, characters, person, ct)).ToList();
        var total = rows.Count;
        var items = rows.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(r => new SearchDto { Tconst = r.Tconst, Title = r.Title, Links = new List<LinkDto> { new("self", $"/api/movies/{r.Tconst}") } })
            .ToList();
        var basePath = $"/api/search/structured?title={Uri.EscapeDataString(title ?? string.Empty)}&plot={Uri.EscapeDataString(plot ?? string.Empty)}&characters={Uri.EscapeDataString(characters ?? string.Empty)}&person={Uri.EscapeDataString(person ?? string.Empty)}";
        var dto = new PageDto<SearchDto> { Page = page, PageSize = pageSize, Total = total, Items = items };
        AddPageLinks(dto, basePath);
        return dto;
    }

    public async Task<PageDto<SearchHistoryDto>> GetSearchHistoryAsync(int userId, int page, int pageSize, CancellationToken ct = default)
    {
        var rows = (await _repo.GetUserSearchHistoryAsync(userId, ct)).ToList();
        var total = rows.Count;
        var items = rows
            .OrderByDescending(r => r.SearchedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new SearchHistoryDto
            {
                Text = r.Text,
                SearchedAt = r.SearchedAt,
                Links = new List<LinkDto> { new("repeat", $"/api/search?query={Uri.EscapeDataString(r.Text)}") }
            })
            .ToList();
        var dto = new PageDto<SearchHistoryDto> { Page = page, PageSize = pageSize, Total = total, Items = items };
        AddPageLinks(dto, $"/api/users/{userId}/history/search?");
        return dto;
    }

    /// <summary>
    /// Adds self/prev/next links. Small but important UX detail for discoverability.
    /// </summary>
    private static void AddPageLinks<T>(PageDto<T> dto, string basePath)
    {
        var sep = basePath.Contains('?') ? (basePath.EndsWith('?') ? string.Empty : "&") : "?";
        dto.Links.Add(new LinkDto("self", $"{basePath}{sep}page={dto.Page}&pageSize={dto.PageSize}"));
        if (dto.Page > 1)
            dto.Links.Add(new LinkDto("prev", $"{basePath}{sep}page={dto.Page - 1}&pageSize={dto.PageSize}"));
        var totalPages = (int)Math.Ceiling(dto.Total / (double)dto.PageSize);
        if (dto.Page < Math.Max(totalPages, 1))
            dto.Links.Add(new LinkDto("next", $"{basePath}{sep}page={dto.Page + 1}&pageSize={dto.PageSize}"));
    }
}
