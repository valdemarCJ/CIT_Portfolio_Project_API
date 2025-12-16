using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Models.Entities;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

/// <summary>
/// Data access for PersonKnownFor resources (read-only list/detail).
/// </summary>
public interface IPersonKnownForRepository
{
    /// <summary>Returns a paged list of person known for entries.</summary>
    Task<PageDto<PersonKnownForDto>> GetPersonKnownForAsync(int page, int pageSize, CancellationToken ct = default);
    /// <summary>Finds a person known for entry by nconst and tconst; null if not found.</summary>
    Task<PersonKnownFor?> GetByIdAsync(string nconst, string tconst, CancellationToken ct = default);
    /// <summary>Returns tconst values for a given nconst (movies known for a person).</summary>
    Task<List<string>> GetMoviesByPersonAsync(string nconst, CancellationToken ct = default);
    /// <summary>Returns nconst values for a given tconst (people known for a movie).</summary>
    Task<List<string>> GetPeopleByMovieAsync(string tconst, CancellationToken ct = default);
}