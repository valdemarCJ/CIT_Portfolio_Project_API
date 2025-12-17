using CIT_Portfolio_Project_API.Infrastructure.Persistence;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;
using CIT_Portfolio_Project_API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Implementations;

public class PersonRatingRepository : IPersonRatingRepository
{
    private readonly AppDbContext _db;
    public PersonRatingRepository(AppDbContext db) => _db = db;

    public async Task RateAsync(int userId, string nconst, int value, CancellationToken ct = default)
    {
        var existingRating = await _db.UserPersonRatings
            .FirstOrDefaultAsync(r => r.UserId == userId && r.Nconst == nconst, ct);

        if (existingRating != null)
        {
            // Update existing rating
            existingRating.Value = value;
        }
        else
        {
            // Create new rating
            var rating = new UserPersonRating
            {
                UserId = userId,
                Nconst = nconst,
                Value = value
            };
            _db.UserPersonRatings.Add(rating);
        }

        await _db.SaveChangesAsync(ct);
    }

    public async Task<List<UserPersonRating>> GetUserPersonRatingHistoryAsync(int userId, CancellationToken ct = default)
    {
        return await _db.UserPersonRatings
            .Where(r => r.UserId == userId)
            .ToListAsync(ct);
    }
}