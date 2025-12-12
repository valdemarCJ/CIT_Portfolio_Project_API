using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CIT_Portfolio_Project_API.Web.Controllers;

[ApiController]
[Route("api/word-index")]
public class WordIndexController : ControllerBase
{
    private readonly IWordIndexManager _manager;

    public WordIndexController(IWordIndexManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        => Ok(await _manager.GetWordIndexAsync(page, pageSize, ct));

    [HttpGet("{tconst}/words")]
    public async Task<IActionResult> GetWordsByMovie(string tconst, CancellationToken ct = default)
        => Ok(await _manager.GetWordsByMovieAsync(tconst, ct));
}