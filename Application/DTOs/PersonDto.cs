namespace CIT_Portfolio_Project_API.Application.DTOs;

/// <summary>
/// Lightweight person representation for API responses (with HATEOAS links).
/// </summary>
public class PersonDto
{
    public string Nconst { get; set; } = default!;
    public string? Name { get; set; }
    public string? BirthYear { get; set; }
    public string? DeathYear { get; set; }
    public decimal? NameRating { get; set; }
    public List<LinkDto> Links { get; set; } = new();
}
