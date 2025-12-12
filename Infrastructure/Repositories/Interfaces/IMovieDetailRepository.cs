using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Models.Entities;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

/// <summary>
/// Data access for MovieDetail resources (read-only).
/// </summary>
public interface IMovieDetailRepository
{
    /// <summary>Finds a movie detail entity by tconst; null if not found.</summary>
    Task<MovieDetail?> GetByIdAsync(string tconst, CancellationToken ct = default);
    /// <summary>Returns a paged list of movie details.</summary>
    Task<PageDto<MovieDetailDto>> GetMovieDetailsAsync(int page, int pageSize, CancellationToken ct = default);
}