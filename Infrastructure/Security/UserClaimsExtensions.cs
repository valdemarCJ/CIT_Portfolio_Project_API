
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace CIT_Portfolio_Project_API.Infrastructure.Security;

public static class UserClaimsExtensions
{
    /// <summary>
    /// Extract user id from JWT claims. Prefers 'sub' (subject),
    /// falls back to NameIdentifier. Returns null if not a valid integer.
    /// </summary>
    public static int? GetUserId(this ClaimsPrincipal user)
    {
        if (user == null) return null;
        var sub = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                  ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(sub, out var id)) return id;
        return null;
    }
}