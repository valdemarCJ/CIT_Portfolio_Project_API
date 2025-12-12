using CIT_Portfolio_Project_API.Infrastructure.Persistence.PgSqlFunctionResults;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

/// <summary>
/// Data access for user bookmarks (CRUD via DB functions).
/// </summary>
public interface IBookmarkRepository
{
    /// <summary>Adds or updates a bookmark with optional note.</summary>
    Task AddAsync(int userId, string tconst, string? note, CancellationToken ct = default);
    /// <summary>Lists bookmarks for the user (title/note shape defined by DB function).</summary>
    Task<IEnumerable<BookmarkRow>> GetAsync(int userId, CancellationToken ct = default);
    /// <summary>Deletes a bookmark if present; no-op otherwise.</summary>
    Task DeleteAsync(int userId, string tconst, CancellationToken ct = default);
}
