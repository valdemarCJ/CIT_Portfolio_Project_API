using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

namespace CIT_Portfolio_Project_API.Application.Managers.Implementations;

public class RatingManager : IRatingManager
{
    private readonly IRatingRepository _repo;
    public RatingManager(IRatingRepository repo) { _repo = repo; }

    /// <summary>
    /// Validates that rating is between 1 and 10 (inclusive). Throws if out of range.
    /// Simple business rule to avoid hitting the DB with invalid values.
    /// </summary>
    public async Task RateAsync(int userId, string tconst, int value, CancellationToken ct = default)
    {
        if (value < 1 || value > 10) throw new ArgumentOutOfRangeException(nameof(value), "Rating must be 1-10");
        await _repo.RateAsync(userId, tconst, value, ct);
    }

    public async Task<PageDto<RatingHistoryDto>> GetRatingHistoryAsync(int userId, int page, int pageSize, CancellationToken ct = default)
    {
        var rows = (await _repo.GetUserRatingHistoryAsync(userId, ct)).ToList();
        var total = rows.Count;
        var items = rows
            .OrderByDescending(r => r.RatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new RatingHistoryDto
            {
                Tconst = r.Tconst,
                Value = r.Value,
                Links = new List<LinkDto> { new("movie", $"/api/movies/{r.Tconst}") }
            })
            .ToList();
        var dto = new PageDto<RatingHistoryDto> { Page = page, PageSize = pageSize, Total = total, Items = items };
        AddPageLinks(dto, $"/api/users/{userId}/history/ratings?");
        return dto;
    }

    private static void AddPageLinks<T>(PageDto<T> dto, string basePath)
    {
        // Keep links valid whether basePath already has query parameters or ends with '?'.
        var sep = basePath.Contains('?') ? (basePath.EndsWith('?') ? string.Empty : "&") : "?";
        dto.Links.Add(new LinkDto("self", $"{basePath}{sep}page={dto.Page}&pageSize={dto.PageSize}"));
        if (dto.Page > 1)
            dto.Links.Add(new LinkDto("prev", $"{basePath}{sep}page={dto.Page - 1}&pageSize={dto.PageSize}"));
        var totalPages = (int)Math.Ceiling(dto.Total / (double)dto.PageSize);
        if (dto.Page < Math.Max(totalPages, 1))
            dto.Links.Add(new LinkDto("next", $"{basePath}{sep}page={dto.Page + 1}&pageSize={dto.PageSize}"));
    }
}
