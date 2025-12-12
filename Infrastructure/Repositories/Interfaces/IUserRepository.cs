using CIT_Portfolio_Project_API.Models.Entities;

namespace CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;

/// <summary>
/// Data access for framework users (lookup, create).
/// </summary>
public interface IUserRepository
{
    /// <summary>Finds a user by username; null if not found.</summary>
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
    /// <summary>Persists a new user (hash already set by caller).</summary>
    Task<User> AddAsync(User user, CancellationToken ct = default);
    /// <summary>Finds a user by id; null if not found.</summary>
    Task<User?> GetByIdAsync(int id, CancellationToken ct = default);
}
