namespace CIT_Portfolio_Project_API.Application.DTOs;

/// <summary>
/// Movie genre representation for API responses (with HATEOAS links).
/// </summary>
public class MovieGenreDto
{
    public string Tconst { get; set; } = default!;
    public string Genre { get; set; } = default!;
    public List<LinkDto> Links { get; set; } = new();
}