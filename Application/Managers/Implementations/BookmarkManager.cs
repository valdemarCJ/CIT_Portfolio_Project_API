using AutoMapper;
using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

namespace CIT_Portfolio_Project_API.Application.Managers.Implementations;

public class BookmarkManager : IBookmarkManager
{
    private readonly IBookmarkRepository _repo;
    private readonly IMapper _mapper;
    public BookmarkManager(IBookmarkRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    /// <summary>
    /// Adds or updates a bookmark with optional note.
    /// Ownership is enforced by passing the authenticated userId from the controller.
    /// </summary>
    public Task AddAsync(int userId, string tconst, string? note, CancellationToken ct = default)
        => _repo.AddAsync(userId, tconst, note, ct);

    /// <summary>
    /// Returns paginated bookmarks for the user with HATEOAS links.
    /// </summary>
    public async Task<PageDto<BookmarkDto>> GetAsync(int userId, int page, int pageSize, CancellationToken ct = default)
    {
        var rows = (await _repo.GetAsync(userId, ct)).ToList();
        var total = rows.Count;
        var items = rows.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(r => new BookmarkDto
            {
                Tconst = r.Tconst,
                Title = r.Title,
                Note = r.Note,
                Links = new List<LinkDto> { new("self", $"/api/movies/{r.Tconst}") }
            })
            .ToList();
        var dto = new PageDto<BookmarkDto> { Page = page, PageSize = pageSize, Total = total, Items = items };
        AddPageLinks(dto, $"/api/users/{userId}/bookmarks");
        return dto;
    }

    /// <summary>
    /// Removes a bookmark if present; no-op otherwise.
    /// </summary>
    public Task DeleteAsync(int userId, string tconst, CancellationToken ct = default)
        => _repo.DeleteAsync(userId, tconst, ct);

    /// <summary>
    /// Adds self/prev/next pagination links.
    /// </summary>
    private static void AddPageLinks<T>(PageDto<T> dto, string basePath)
    {
        var sep = basePath.Contains('?') ? "&" : "?";
        dto.Links.Add(new LinkDto("self", $"{basePath}{sep}page={dto.Page}&pageSize={dto.PageSize}"));
        if (dto.Page > 1)
            dto.Links.Add(new LinkDto("prev", $"{basePath}{sep}page={dto.Page - 1}&pageSize={dto.PageSize}"));
        var totalPages = (int)Math.Ceiling(dto.Total / (double)dto.PageSize);
        if (dto.Page < Math.Max(totalPages, 1))
            dto.Links.Add(new LinkDto("next", $"{basePath}{sep}page={dto.Page + 1}&pageSize={dto.PageSize}"));
    }
}
