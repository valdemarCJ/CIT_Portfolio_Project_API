using CIT_Portfolio_Project_API.Infrastructure.Persistence;
using CIT_Portfolio_Project_API.Infrastructure.Persistence.PgSqlFunctionResults;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Implementations;

/// <summary>
/// Analytics queries delegated to DB functions (popular actors, similarity, words, etc.).
/// </summary>
public class AnalyticsRepository : IAnalyticsRepository
{
    private readonly AppDbContext _db;
    public AnalyticsRepository(AppDbContext db) => _db = db;

    /// <summary>Popular actors for a given title (DB function call).</summary>
    public Task<IEnumerable<PopularActorRow>> PopularActorsInMovieAsync(string tconst, CancellationToken ct = default)
        => _db.CallPopularActorsInMovie(tconst).ToListAsync(ct).ContinueWith(t => (IEnumerable<PopularActorRow>)t.Result, ct);

    /// <summary>Similar movies for a given title (DB function call).</summary>
    public Task<IEnumerable<SimilarTitleRow>> SimilarMoviesAsync(string tconst, CancellationToken ct = default)
        => _db.CallSimilarMovies(tconst).ToListAsync(ct).ContinueWith(t => (IEnumerable<SimilarTitleRow>)t.Result, ct);

    /// <summary>Top words for people matching the given name (DB function call).</summary>
    public Task<IEnumerable<PersonWordRow>> PersonWordsAsync(string name, CancellationToken ct = default)
        => _db.CallPersonWords(name).ToListAsync(ct).ContinueWith(t => (IEnumerable<PersonWordRow>)t.Result, ct);

    /// <summary>Co-players for the given actor (DB function call).</summary>
    public Task<IEnumerable<CoPlayerRow>> CoPlayersAsync(string actor, CancellationToken ct = default)
        => _db.CallCoPlayers(actor).ToListAsync(ct).ContinueWith(t => (IEnumerable<CoPlayerRow>)t.Result, ct);

    public Task<IEnumerable<ExactMatchRow>> ExactMatchAsync(string query, CancellationToken ct = default)
    {
        var parts = (query ?? string.Empty)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Take(3)
            .ToArray();
        string? w1 = parts.ElementAtOrDefault(0);
        string? w2 = parts.ElementAtOrDefault(1);
        string? w3 = parts.ElementAtOrDefault(2);
        return _db.CallExactMatch(w1, w2, w3).ToListAsync(ct).ContinueWith(t => (IEnumerable<ExactMatchRow>)t.Result, ct);
    }

    public Task<IEnumerable<BestMatchRow>> BestMatchAsync(string query, CancellationToken ct = default)
    {
        var parts = (query ?? string.Empty)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Take(3)
            .ToArray();
        string? w1 = parts.ElementAtOrDefault(0);
        string? w2 = parts.ElementAtOrDefault(1);
        string? w3 = parts.ElementAtOrDefault(2);
        return _db.CallBestMatch(w1, w2, w3).ToListAsync(ct).ContinueWith(t => (IEnumerable<BestMatchRow>)t.Result, ct);
    }
}
