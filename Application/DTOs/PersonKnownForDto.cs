namespace CIT_Portfolio_Project_API.Application.DTOs;

/// <summary>
/// Lightweight person known for representation for API responses (with HATEOAS links).
/// </summary>
public class PersonKnownForDto
{
    public string Nconst { get; set; } = default!;
    public string Tconst { get; set; } = default!;
    public List<LinkDto> Links { get; set; } = new();
}