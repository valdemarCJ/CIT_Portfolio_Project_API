using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Models.Entities;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

/// <summary>
/// Data access for MovieGenre resources (read-only).
/// </summary>
public interface IMovieGenreRepository
{
    /// <summary>Returns all genres for a specific movie.</summary>
    Task<List<MovieGenre>> GetGenresByMovieAsync(string tconst, CancellationToken ct = default);
    /// <summary>Returns a paged list of movie genres.</summary>
    Task<PageDto<MovieGenreDto>> GetMovieGenresAsync(int page, int pageSize, CancellationToken ct = default);
}