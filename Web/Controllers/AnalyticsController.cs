using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CIT_Portfolio_Project_API.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsManager _manager;
    public AnalyticsController(IAnalyticsManager manager) { _manager = manager; }

    [HttpGet("popular-actors/{tconst}")]
    public async Task<IActionResult> PopularActors(string tconst, CancellationToken ct)
    => Ok(await _manager.PopularActorsInMovieAsync(tconst, ct));

    [HttpGet("similar/{tconst}")]
    public async Task<IActionResult> Similar(string tconst, CancellationToken ct)
    => Ok(await _manager.SimilarMoviesAsync(tconst, ct));

    [HttpGet("person-words")]
    public async Task<IActionResult> PersonWords([FromQuery] string name, CancellationToken ct)
    => Ok(await _manager.PersonWordsAsync(name, ct));

    [HttpGet("co-players")]
    public async Task<IActionResult> CoPlayers([FromQuery] string actor, CancellationToken ct)
    => Ok(await _manager.CoPlayersAsync(actor, ct));

    [HttpGet("exact-match")]
    public async Task<IActionResult> ExactMatch([FromQuery] string query, CancellationToken ct)
    => Ok(await _manager.ExactMatchAsync(query, ct));

    [HttpGet("best-match")]
    public async Task<IActionResult> BestMatch([FromQuery] string query, CancellationToken ct)
    => Ok(await _manager.BestMatchAsync(query, ct));
}
