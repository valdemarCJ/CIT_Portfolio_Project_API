using CIT_Portfolio_Project_API.Application.DTOs.Auth;
using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CIT_Portfolio_Project_API.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // Handles login and issues JWTs; delegates credential checks to the auth manager.
    private readonly IAuthManager _auth;
    public AuthController(IAuthManager auth) { _auth = auth; }

    /// <summary>
    /// Login with username and password to get a JWT.
    /// </summary>
    /// <param name="request">Login request with username and password.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>JWT token and user information on success.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody][Required] LoginRequest request,
        CancellationToken ct)
    {
        // Delegate to auth manager; return 401 if credentials are invalid.
        var res = await _auth.LoginAsync(request, ct);
        return res is null ? Unauthorized() : Ok(res);
    }
}
