using CIT_Portfolio_Project_API.Application.Managers.Interfaces;
using CIT_Portfolio_Project_API.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIT_Portfolio_Project_API.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    // Exposes user registration and user-scoped history endpoints. Business logic is delegated to managers.
    private readonly IUserManager _manager;
    private readonly ISearchManager _searchManager;
    private readonly IRatingManager _ratingManager;
    public UsersController(IUserManager manager, ISearchManager searchManager, IRatingManager ratingManager)
    { _manager = manager; _searchManager = searchManager; _ratingManager = ratingManager; }

    public record RegisterRequest(string Username, string Email, string Password);

    [HttpPost]
    // Register a new user; returns 201 Created with the Location header pointing to the new resource.
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var dto = await _manager.RegisterAsync(request.Username, request.Email, request.Password, ct);
        return Created($"/api/users/{dto.Id}", dto);
    }

    [HttpGet("{id:int}")]
    // Get a user by id. 404 if not found.
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var dto = await _manager.GetByIdAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet("{id:int}/history/search")]
    [Authorize]
    public async Task<IActionResult> GetSearchHistory(int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        // Auth + ownership: only the token owner may access their own history.
        var tokenUserId = User.GetUserId();
        if (tokenUserId is null || tokenUserId <= 0) return Unauthorized();
        if (tokenUserId.Value != id) return Forbid();
        var dto = await _searchManager.GetSearchHistoryAsync(id, page, pageSize, ct);
        return Ok(dto);
    }

    [HttpGet("{id:int}/history/ratings")]
    [Authorize]
    public async Task<IActionResult> GetRatingHistory(int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        // Auth + ownership: only the token owner may access their own history.
        var tokenUserId = User.GetUserId();
        if (tokenUserId is null || tokenUserId <= 0) return Unauthorized();
        if (tokenUserId.Value != id) return Forbid();
        var dto = await _ratingManager.GetRatingHistoryAsync(id, page, pageSize, ct);
        return Ok(dto);
    }
}
