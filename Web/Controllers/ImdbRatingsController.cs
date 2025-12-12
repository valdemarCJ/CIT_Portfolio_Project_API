using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CIT_Portfolio_Project_API.Web.Controllers;

[ApiController]
[Route("api/imdb-ratings")]
public class ImdbRatingsController : ControllerBase
{
    private readonly IRatingReadManager _manager;

    public ImdbRatingsController(IRatingReadManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        => Ok(await _manager.GetRatingsAsync(page, pageSize, ct));

    [HttpGet("{tconst}")]
    public async Task<IActionResult> GetById(string tconst, CancellationToken ct = default)
    {
        var dto = await _manager.GetByIdAsync(tconst, ct);
        return dto is null ? NotFound() : Ok(dto);
    }
}