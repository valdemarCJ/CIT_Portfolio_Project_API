using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CIT_Portfolio_Project_API.Web.Controllers;

[ApiController]
[Route("api/users/{userId:int}/person-bookmarks")]
[Authorize]
public class PersonBookmarksController : ControllerBase
{
    private readonly IPersonBookmarkManager _manager;
    public PersonBookmarksController(IPersonBookmarkManager manager) { _manager = manager; }

    /// <summary>
    /// Get all person bookmarks for a user
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get(int userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var tokenUserId = User.GetUserId();
        if (tokenUserId is null || tokenUserId <= 0) return Unauthorized();
        if (tokenUserId.Value != userId) return Forbid();
        return Ok(await _manager.GetAsync(tokenUserId.Value, page, pageSize, ct));
    }

    /// <summary>
    /// Add a person to user's bookmarks
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Add(
        [FromRoute] int userId,
        [FromQuery][Required][RegularExpression(@"^nm\d+$", ErrorMessage = "Person ID must start with 'nm' followed by numbers")] string nconst,
        [FromQuery] string? note,
        CancellationToken ct)
    {
        var tokenUserId = User.GetUserId();
        if (tokenUserId is null || tokenUserId <= 0) return Unauthorized();
        if (tokenUserId.Value != userId) return Forbid();
        await _manager.AddAsync(tokenUserId.Value, nconst, note, ct);
        return NoContent();
    }

    [HttpDelete("{nconst}")]
    public async Task<IActionResult> Delete(int userId, string nconst, CancellationToken ct)
    {
        var tokenUserId = User.GetUserId();
        if (tokenUserId is null || tokenUserId <= 0) return Unauthorized();
        if (tokenUserId.Value != userId) return Forbid();
        await _manager.DeleteAsync(tokenUserId.Value, nconst, ct);
        return NoContent();
    }
}