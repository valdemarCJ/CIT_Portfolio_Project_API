using CIT_Portfolio_Project_API.Infrastructure.Persistence;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Implementations;

/// <summary>
/// Rating persistence via DB-side functions (write + history read).
/// </summary>
public class RatingRepository : IRatingRepository
{
    private readonly AppDbContext _db;
    public RatingRepository(AppDbContext db) => _db = db;

    /// <summary>Creates or updates a rating for a movie and user.</summary>
    public async Task RateAsync(int userId, string tconst, int value, CancellationToken ct = default)
    {
        await _db.ExecuteRateAsync(userId, tconst, value, ct);
    }

    /// <summary>Gets rating history rows for a user.</summary>
    public async Task<IEnumerable<CIT_Portfolio_Project_API.Infrastructure.Persistence.PgSqlFunctionResults.UserRatingHistoryRow>> GetUserRatingHistoryAsync(int userId, CancellationToken ct = default)
        => await _db.CallUserRatingHistory(userId).ToListAsync(ct);
}
