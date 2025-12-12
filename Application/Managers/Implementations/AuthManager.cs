using CIT_Portfolio_Project_API.Application.DTOs.Auth;
using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Repositories.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Security;

namespace CIT_Portfolio_Project_API.Application.Managers.Implementations;

public class AuthManager : IAuthManager
{
    private readonly IUserRepository _users;
    private readonly PasswordHasher _hasher;
    private readonly JwtTokenService _jwt;
    public AuthManager(IUserRepository users, PasswordHasher hasher, JwtTokenService jwt)
    { _users = users; _hasher = hasher; _jwt = jwt; }

    /// <summary>
    /// Authenticates a user and issues a short-lived JWT on success.
    /// Returns null on invalid credentials to avoid leaking which field failed.
    /// </summary>
    public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await _users.GetByUsernameAsync(request.Username, ct);
        if (user == null) return null;
        if (!_hasher.Verify(request.Password, user.PasswordHash)) return null;
        var (token, expires) = _jwt.GenerateToken(user);
        return new LoginResponse { Token = token, ExpiresAt = expires };
    }
}
