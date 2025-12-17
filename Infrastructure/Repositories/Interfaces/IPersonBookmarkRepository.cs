using CIT_Portfolio_Project_API.Models.Entities;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

public interface IPersonBookmarkRepository
{
    Task AddAsync(int userId, string nconst, string? note, CancellationToken ct = default);
    Task<List<UserPersonBookmark>> GetUserPersonBookmarksAsync(int userId, CancellationToken ct = default);
    Task DeleteAsync(int userId, string nconst, CancellationToken ct = default);
}