using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CIT_Portfolio_Project_API.Web.Controllers;

[ApiController]
[Route("api/users/{userId:int}/[controller]")]
[Authorize]
public class BookmarksController : ControllerBase
{
    // Manages user-scoped bookmark operations; requires JWT and enforces ownership.
    private readonly IBookmarkManager _manager;
    public BookmarksController(IBookmarkManager manager) { _manager = manager; }

    /// <summary>
    /// Get all bookmarks for a user
    /// </summary>
    /// <param name="userId">ID of the user whose bookmarks to retrieve</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of bookmarks with movie information</returns>
    [HttpGet]
    public async Task<IActionResult> Get(int userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        // Ensure the caller is authenticated and only accesses their own bookmarks.
        var tokenUserId = User.GetUserId();
        if (tokenUserId is null || tokenUserId <= 0) return Unauthorized();
        if (tokenUserId.Value != userId) return Forbid();
        return Ok(await _manager.GetAsync(tokenUserId.Value, page, pageSize, ct));
    }


    /// <summary>
    /// Add a movie to user's bookmarks
    /// </summary>
    /// <param name="userId">ID of the user to add bookmark for</param>
    /// <param name="tconst">IMDB ID of the movie to bookmark (starts with 'tt')</param>
    /// <param name="note">Optional note about the bookmark</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>No content on success</returns>
    [HttpPost]
    public async Task<IActionResult> Add(
        [FromRoute] int userId,
        [FromQuery][Required][RegularExpression(@"^tt\d+$", ErrorMessage = "Movie ID must start with 'tt' followed by numbers")] string tconst,
        [FromQuery] string? note,
        CancellationToken ct)
    {
        // Enforce authentication and ownership before creating a bookmark.
        var tokenUserId = User.GetUserId();
        if (tokenUserId is null || tokenUserId <= 0) return Unauthorized();
        if (tokenUserId.Value != userId) return Forbid();
        await _manager.AddAsync(tokenUserId.Value, tconst, note, ct);
        return NoContent();
    }

    [HttpDelete("{tconst}")]
    public async Task<IActionResult> Delete(int userId, string tconst, CancellationToken ct)
    {
        // Enforce authentication and ownership before deletion.
        var tokenUserId = User.GetUserId();
        if (tokenUserId is null || tokenUserId <= 0) return Unauthorized();
        if (tokenUserId.Value != userId) return Forbid();
        await _manager.DeleteAsync(tokenUserId.Value, tconst, ct);
        return NoContent();
    }

    /// <summary>
    /// Request model for adding a bookmark (used by Swagger).
    /// </summary>
    public record AddBookmarkRequest(string Tconst, string? Note);
}
