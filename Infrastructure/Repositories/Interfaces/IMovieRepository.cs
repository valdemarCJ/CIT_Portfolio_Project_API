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
}
