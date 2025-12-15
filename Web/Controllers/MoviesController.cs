using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace CIT_Portfolio_Project_API.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    // Exposes read-only movie resources (list, details, search) and delegates to the movie manager.
    private readonly IMovieManager _manager;
    private readonly ISearchManager _searchManager;
    public MoviesController(IMovieManager manager, ISearchManager searchManager) { _manager = manager; _searchManager = searchManager; }

    [HttpGet]
    // List movies with pagination (page defaults to 1, pageSize to 20).
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    => Ok(await _manager.GetMoviesAsync(page, pageSize, ct));

    [HttpGet("{tconst}")]
    // Get a single movie by its stable ID (tconst). 404 if not found.
    public async Task<IActionResult> GetById(string tconst, CancellationToken ct)
    {
        var dto = await _manager.GetByIdAsync(tconst, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet("search")]
    [Authorize]
    public async Task<IActionResult> Search([FromQuery, DefaultValue("dark knight")] string query, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var userId = User.GetUserId();
        if (userId is null || userId <= 0) return Unauthorized();
        return Ok(await _searchManager.StringSearchAsync(userId.Value, query, page, pageSize, ct));
    }

    [HttpGet("structured")]
    [Authorize]
    public async Task<IActionResult> Structured([FromQuery, DefaultValue("dark knight")] string? title, [FromQuery, DefaultValue("")] string? plot, [FromQuery, DefaultValue("")] string? characters, [FromQuery, DefaultValue("Christian Bale")] string? person, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var userId = User.GetUserId();
        if (userId is null || userId <= 0) return Unauthorized();
        return Ok(await _searchManager.StructuredSearchAsync(userId.Value, title, plot, characters, person, page, pageSize, ct));
    }

    [HttpGet("types/movies")]
    // Get movies filtered by movie types (movie, tvMovie, short)
    public async Task<IActionResult> GetMovieTypes([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    => Ok(await _manager.GetMovieTypesAsync(page, pageSize, ct));

    [HttpGet("types/series")]
    // Get movies filtered by series types (tvMiniSeries, tvEpisode, tvSeries)
    public async Task<IActionResult> GetSeriesTypes([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    => Ok(await _manager.GetSeriesTypesAsync(page, pageSize, ct));

    [HttpGet("types/special")]
    // Get movies filtered by special types (tvShort, videoGame, video, tvSpecial)
    public async Task<IActionResult> GetSpecialTypes([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    => Ok(await _manager.GetSpecialTypesAsync(page, pageSize, ct));
}
