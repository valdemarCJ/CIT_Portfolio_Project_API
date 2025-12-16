using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CIT_Portfolio_Project_API.Web.Controllers;

[ApiController]
[Route("api/person-knownfor")]
public class PersonKnownForController : ControllerBase
{
    // Exposes read-only person known for resources (list, details) and delegates to the person known for manager.
    private readonly IPersonKnownForManager _manager;
    public PersonKnownForController(IPersonKnownForManager manager) { _manager = manager; }

    [HttpGet]
    // List person known for entries with pagination (page defaults to 1, pageSize to 20).
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    => Ok(await _manager.GetPersonKnownForAsync(page, pageSize, ct));

    [HttpGet("{nconst}/{tconst}")]
    // Get a single person known for entry by its composite key (nconst and tconst). 404 if not found.
    public async Task<IActionResult> GetById(string nconst, string tconst, CancellationToken ct)
    {
        var dto = await _manager.GetByIdAsync(nconst, tconst, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet("by-person/{nconst}")]
    // Get movies (tconst values) that a person is known for
    public async Task<IActionResult> GetMoviesByPerson(string nconst, CancellationToken ct)
    {
        var movies = await _manager.GetMoviesByPersonAsync(nconst, ct);
        return Ok(movies);
    }

    [HttpGet("by-movie/{tconst}")]
    // Get people (nconst values) that are known for a movie
    public async Task<IActionResult> GetPeopleByMovie(string tconst, CancellationToken ct)
    {
        var people = await _manager.GetPeopleByMovieAsync(tconst, ct);
        return Ok(people);
    }
}