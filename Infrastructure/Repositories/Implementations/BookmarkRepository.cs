using CIT_Portfolio_Project_API.Infrastructure.Persistence;
using CIT_Portfolio_Project_API.Infrastructure.Persistence.PgSqlFunctionResults;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Implementations;

/// <summary>
/// Bookmark operations via DB functions (add/list/delete).
/// </summary>
public class BookmarkRepository : IBookmarkRepository
{
    private readonly AppDbContext _db;
    public BookmarkRepository(AppDbContext db) => _db = db;

    /// <summary>Adds or updates a bookmark with optional note.</summary>
    public async Task AddAsync(int userId, string tconst, string? note, CancellationToken ct = default)
    {
        await _db.ExecuteAddBookmarkAsync(userId, tconst, note, ct);
    }

    /// <summary>Lists bookmarks for a user.</summary>
    public async Task<IEnumerable<BookmarkRow>> GetAsync(int userId, CancellationToken ct = default)
    {
        return await _db.CallGetBookmarks(userId).ToListAsync(ct);
    }

    /// <summary>Removes a bookmark for a user (no-op if not present).</summary>
    public async Task DeleteAsync(int userId, string tconst, CancellationToken ct = default)
    {
        var bookmark = await _db.UserBookmarks
            .FirstOrDefaultAsync(b => b.UserId == userId && b.Tconst == tconst, ct);
        if (bookmark != null)
        {
            _db.UserBookmarks.Remove(bookmark);
            await _db.SaveChangesAsync(ct);
        }
    }
}
