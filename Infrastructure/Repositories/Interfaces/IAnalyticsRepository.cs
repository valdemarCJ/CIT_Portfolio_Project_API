using CIT_Portfolio_Project_API.Infrastructure.Persistence.PgSqlFunctionResults;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

/// <summary>
/// Data access for analytics endpoints backed by database functions.
/// </summary>
public interface IAnalyticsRepository
{
    /// <summary>Popular actors for a given title.</summary>
    Task<IEnumerable<PopularActorRow>> PopularActorsInMovieAsync(string tconst, CancellationToken ct = default);
    /// <summary>Movies similar to the given title.</summary>
    Task<IEnumerable<SimilarTitleRow>> SimilarMoviesAsync(string tconst, CancellationToken ct = default);
    /// <summary>Relevant words associated with matching people for a name.</summary>
    Task<IEnumerable<PersonWordRow>> PersonWordsAsync(string name, CancellationToken ct = default);
    /// <summary>People who co-star with the given actor.</summary>
    Task<IEnumerable<CoPlayerRow>> CoPlayersAsync(string actor, CancellationToken ct = default);
    /// <summary>Exact match titles for a textual query (up to three tokens).</summary>
    Task<IEnumerable<ExactMatchRow>> ExactMatchAsync(string query, CancellationToken ct = default);
    /// <summary>Best match titles ranked for a textual query (up to three tokens).</summary>
    Task<IEnumerable<BestMatchRow>> BestMatchAsync(string query, CancellationToken ct = default);
}
