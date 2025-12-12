namespace CIT_Portfolio_Project_API.Application.DTOs;

/// <summary>
/// A user's bookmark of a movie (optional note and links included).
/// </summary>
public class BookmarkDto
{
    public string Tconst { get; set; } = default!;
    public string? Title { get; set; }
    public string? Note { get; set; }
    public List<LinkDto> Links { get; set; } = new();
}
