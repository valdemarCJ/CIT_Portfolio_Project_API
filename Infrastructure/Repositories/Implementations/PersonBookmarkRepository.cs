using CIT_Portfolio_Project_API.Infrastructure.Persistence;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;
using CIT_Portfolio_Project_API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Implementations;

public class PersonBookmarkRepository : IPersonBookmarkRepository
{
    private readonly AppDbContext _db;
    public PersonBookmarkRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(int userId, string nconst, string? note, CancellationToken ct = default)
    {
        var bookmark = new UserPersonBookmark
        {
            UserId = userId,
            Nconst = nconst,
            Note = note
        };
        
        _db.UserPersonBookmarks.Add(bookmark);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<List<UserPersonBookmark>> GetUserPersonBookmarksAsync(int userId, CancellationToken ct = default)
    {
        return await _db.UserPersonBookmarks
            .Where(b => b.UserId == userId)
            .ToListAsync(ct);
    }

    public async Task DeleteAsync(int userId, string nconst, CancellationToken ct = default)
    {
        var bookmark = await _db.UserPersonBookmarks
            .FirstOrDefaultAsync(b => b.UserId == userId && b.Nconst == nconst, ct);
        
        if (bookmark != null)
        {
            _db.UserPersonBookmarks.Remove(bookmark);
            await _db.SaveChangesAsync(ct);
        }
    }
}