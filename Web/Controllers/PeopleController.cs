using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CIT_Portfolio_Project_API.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeopleController : ControllerBase
{
    // Exposes read-only person resources (list and details) and delegates to the person manager.
    private readonly IPersonManager _manager;
    public PeopleController(IPersonManager manager) { _manager = manager; }

    [HttpGet]
    // List people with pagination.
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    => Ok(await _manager.GetPeopleAsync(page, pageSize, ct));

    [HttpGet("{nconst}")]
    // Get a single person by their stable ID (nconst). 404 if not found.
    public async Task<IActionResult> GetById(string nconst, CancellationToken ct)
    {
        var dto = await _manager.GetByIdAsync(nconst, ct);
        return dto is null ? NotFound() : Ok(dto);
    }
}
