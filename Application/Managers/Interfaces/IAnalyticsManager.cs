using CIT_Portfolio_Project_API.Application.DTOs;

namespace CIT_Portfolio_Project_API.Application.Managers.Interfaces;

public interface IAnalyticsManager
{
    Task<IEnumerable<PersonDto>> PopularActorsInMovieAsync(string tconst, CancellationToken ct = default);
    Task<IEnumerable<MovieDto>> SimilarMoviesAsync(string tconst, CancellationToken ct = default);
    Task<IEnumerable<string>> PersonWordsAsync(string name, CancellationToken ct = default);
    Task<IEnumerable<PersonDto>> CoPlayersAsync(string actor, CancellationToken ct = default);
    Task<IEnumerable<MovieDto>> ExactMatchAsync(string query, CancellationToken ct = default);
    Task<IEnumerable<MovieDto>> BestMatchAsync(string query, CancellationToken ct = default);
}
