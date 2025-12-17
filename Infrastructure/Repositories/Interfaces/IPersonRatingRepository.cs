using CIT_Portfolio_Project_API.Models.Entities;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

public interface IPersonRatingRepository
{
    Task RateAsync(int userId, string nconst, int value, CancellationToken ct = default);
    Task<List<UserPersonRating>> GetUserPersonRatingHistoryAsync(int userId, CancellationToken ct = default);
}