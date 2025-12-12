namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

/// <summary>
/// Data access for user ratings (write + history reads via DB functions).
/// </summary>
public interface IRatingRepository
{
    /// <summary>Creates or updates a rating value for a movie.</summary>
    Task RateAsync(int userId, string tconst, int value, CancellationToken ct = default);
    /// <summary>Gets rating history rows for the user (most recent first at call site).</summary>
    Task<IEnumerable<CIT_Portfolio_Project_API.Infrastructure.Persistence.PgSqlFunctionResults.UserRatingHistoryRow>> GetUserRatingHistoryAsync(int userId, CancellationToken ct = default);
}
