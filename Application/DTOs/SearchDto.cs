namespace CIT_Portfolio_Project_API.Application.DTOs;

/// <summary>
/// Result row for search endpoints (movie reference + links).
/// </summary>
public class SearchDto
{
    public string Tconst { get; set; } = default!;
    public string Title { get; set; } = default!;
    public List<LinkDto> Links { get; set; } = new();
}
