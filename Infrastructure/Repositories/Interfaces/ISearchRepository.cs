using CIT_Portfolio_Project_API.Infrastructure.Persistence.PgSqlFunctionResults;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

/// <summary>
/// Data access for search operations (free-text, structured, and user history).
/// </summary>
public interface ISearchRepository
{
    /// <summary>Executes free-text search and logs to user history server-side.</summary>
    Task<IEnumerable<SearchRow>> StringSearchAsync(int userId, string text, CancellationToken ct = default);
    /// <summary>Executes structured search and logs to user history server-side.</summary>
    Task<IEnumerable<StructuredSearchRow>> StructuredSearchAsync(int userId, string? title, string? plot, string? characters, string? person, CancellationToken ct = default);
    /// <summary>Returns search history rows for a user.</summary>
    Task<IEnumerable<UserSearchHistoryRow>> GetUserSearchHistoryAsync(int userId, CancellationToken ct = default);
}
