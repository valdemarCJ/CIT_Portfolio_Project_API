using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Models.Entities;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

/// <summary>
/// Data access for Movie resources (read-only list/detail and search projections).
/// </summary>
public interface IMovieRepository
{
    /// <summary>Returns a paged list of movies.</summary>
    Task<PageDto<MovieDto>> GetMoviesAsync(int page, int pageSize, CancellationToken ct = default);
    /// <summary>Finds a movie entity by tconst; null if not found.</summary>
    Task<Movie?> GetByIdAsync(string tconst, CancellationToken ct = default);
    /// <summary>Returns paged movies filtered by movie types (movie, tvMovie, short).</summary>
    Task<PageDto<MovieDto>> GetMovieTypesAsync(int page, int pageSize, CancellationToken ct = default);
    /// <summary>Returns paged movies filtered by series types (tvMiniSeries, tvEpisode, tvSeries).</summary>
    Task<PageDto<MovieDto>> GetSeriesTypesAsync(int page, int pageSize, CancellationToken ct = default);
    /// <summary>Returns paged movies filtered by special types (tvShort, videoGame, video, tvSpecial).</summary>
    Task<PageDto<MovieDto>> GetSpecialTypesAsync(int page, int pageSize, CancellationToken ct = default);
}
