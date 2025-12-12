using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CIT_Portfolio_Project_API.Web.Controllers;

[ApiController]
[Route("api/movie-genres")]
public class MovieGenresController : ControllerBase
{
    private readonly IMovieGenreManager _manager;

    public MovieGenresController(IMovieGenreManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        => Ok(await _manager.GetMovieGenresAsync(page, pageSize, ct));

    [HttpGet("{tconst}/genres")]
    public async Task<IActionResult> GetGenresByMovie(string tconst, CancellationToken ct = default)
        => Ok(await _manager.GetGenresByMovieAsync(tconst, ct));
}