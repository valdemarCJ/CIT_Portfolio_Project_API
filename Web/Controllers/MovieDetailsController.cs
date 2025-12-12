using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CIT_Portfolio_Project_API.Web.Controllers;

[ApiController]
[Route("api/movie-details")]
public class MovieDetailsController : ControllerBase
{
    private readonly IMovieDetailManager _manager;

    public MovieDetailsController(IMovieDetailManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        => Ok(await _manager.GetMovieDetailsAsync(page, pageSize, ct));

    [HttpGet("{tconst}")]
    public async Task<IActionResult> GetById(string tconst, CancellationToken ct = default)
    {
        var dto = await _manager.GetByIdAsync(tconst, ct);
        return dto is null ? NotFound() : Ok(dto);
    }
}