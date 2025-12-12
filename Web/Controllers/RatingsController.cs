using CIT_Portfolio_Project_API.Application.DTOs;
using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CIT_Portfolio_Project_API.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RatingsController : ControllerBase
{
    // Handles authenticated rating operations (rate a movie). Delegates business rules to the rating manager.
    private readonly IRatingManager _ratingManager;
    public RatingsController(IRatingManager ratingManager) { _ratingManager = ratingManager; }

    /// <summary>
    /// Rate a movie with a value between 1 and 10 (inclusive).
    /// </summary>
    /// <param name="tconst">IMDB tconst (starts with 'tt').</param>
    /// <param name="value">Rating value (1-10).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content on success.</returns>
    [HttpPost]
    public async Task<IActionResult> Rate(
        [FromQuery][Required][RegularExpression(@"^tt\d+$", ErrorMessage = "Tconst must start with 'tt' followed by digits")] string tconst,
        [FromQuery][Required][Range(1, 10, ErrorMessage = "Rating must be between 1 and 10")] int value,
        CancellationToken ct)
    {
        // Require authenticated user and ensure we can resolve their id from the JWT.
        var userId = User.GetUserId();
        if (userId is null || userId <= 0) return Unauthorized();
        // Business rule (1..10) is validated here and enforced again in the manager.
        await _ratingManager.RateAsync(userId.Value, tconst, value, ct);
        return NoContent();
    }
}
