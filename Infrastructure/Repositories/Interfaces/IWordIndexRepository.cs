using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Models.Entities;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

/// <summary>
/// Data access for WordIndex resources (read-only).
/// </summary>
public interface IWordIndexRepository
{
    /// <summary>Returns all word index entries for a specific movie.</summary>
    Task<List<WordIndex>> GetWordsByMovieAsync(string tconst, CancellationToken ct = default);
    /// <summary>Returns a paged list of word index entries.</summary>
    Task<PageDto<WordIndexDto>> GetWordIndexAsync(int page, int pageSize, CancellationToken ct = default);
}