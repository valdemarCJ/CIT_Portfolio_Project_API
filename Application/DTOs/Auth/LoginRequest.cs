namespace CIT_Portfolio_Project_API.Application.DTOs.Auth;

/// <summary>
/// Login request payload.
/// </summary>
public class LoginRequest
{
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}
