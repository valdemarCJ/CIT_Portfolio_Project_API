namespace CIT_Portfolio_Project_API.Application.DTOs;

/// <summary>
/// Movie person representation for API responses (with HATEOAS links).
/// </summary>
public class MoviePersonDto
{
    public string Tconst { get; set; } = default!;
    public string Nconst { get; set; } = default!;
    public string? Role { get; set; }
    public int Ordering { get; set; }
    public string? Category { get; set; }
    public string? Job { get; set; }
    public string? Characters { get; set; }
    public List<LinkDto> Links { get; set; } = new();
}