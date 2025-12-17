using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CIT_Portfolio_Project_API.Web.Controllers;

[ApiController]
[Route("api/person-ratings")]
[Authorize]
public class PersonRatingsController : ControllerBase
{
    private readonly IPersonRatingManager _manager;
    public PersonRatingsController(IPersonRatingManager manager) { _manager = manager; }

    /// <summary>
    /// Rate a person with a value between 1 and 10 (inclusive).
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Rate(
        [FromQuery][Required][RegularExpression(@"^nm\d+$", ErrorMessage = "Nconst must start with 'nm' followed by digits")] string nconst,
        [FromQuery][Required][Range(1, 10, ErrorMessage = "Rating must be between 1 and 10")] int value,
        CancellationToken ct)
    {
        var userId = User.GetUserId();
        if (userId is null || userId <= 0) return Unauthorized();
        await _manager.RateAsync(userId.Value, nconst, value, ct);
        return NoContent();
    }
}