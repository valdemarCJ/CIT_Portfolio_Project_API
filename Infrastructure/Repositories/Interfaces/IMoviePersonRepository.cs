using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Models.Entities;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

/// <summary>
/// Data access for MoviePerson resources (read-only).
/// </summary>
public interface IMoviePersonRepository
{
    /// <summary>Returns all people associated with a specific movie.</summary>
    Task<List<MoviePerson>> GetPeopleByMovieAsync(string tconst, CancellationToken ct = default);
    /// <summary>Returns a paged list of movie people.</summary>
    Task<PageDto<MoviePersonDto>> GetMoviePeopleAsync(int page, int pageSize, CancellationToken ct = default);
}