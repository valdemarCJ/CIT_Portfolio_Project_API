using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Models.Entities;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

/// <summary>
/// Data access for IMDB Rating resources (read-only).
/// </summary>
public interface IRatingReadRepository
{
    /// <summary>Finds a rating entity by tconst; null if not found.</summary>
    Task<Rating?> GetByIdAsync(string tconst, CancellationToken ct = default);
    /// <summary>Returns a paged list of IMDB ratings.</summary>
    Task<PageDto<ImdbRatingDto>> GetRatingsAsync(int page, int pageSize, CancellationToken ct = default);
}