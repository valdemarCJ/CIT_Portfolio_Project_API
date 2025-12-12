namespace CIT_Portfolio_Project_API.Application.DTOs.Auth;

/// <summary>
/// JWT token response payload.
/// </summary>
public class LoginResponse
{
    public string Token { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
}
