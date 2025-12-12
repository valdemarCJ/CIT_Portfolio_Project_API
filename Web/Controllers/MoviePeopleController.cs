using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CIT_Portfolio_Project_API.Web.Controllers;

[ApiController]
[Route("api/movie-people")]
public class MoviePeopleController : ControllerBase
{
    private readonly IMoviePersonManager _manager;

    public MoviePeopleController(IMoviePersonManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        => Ok(await _manager.GetMoviePeopleAsync(page, pageSize, ct));

    [HttpGet("{tconst}/people")]
    public async Task<IActionResult> GetPeopleByMovie(string tconst, CancellationToken ct = default)
        => Ok(await _manager.GetPeopleByMovieAsync(tconst, ct));
}