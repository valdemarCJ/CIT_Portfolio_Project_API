namespace CIT_Portfolio_Project_API.Application.DTOs;

/// <summary>
/// Word index representation for API responses (with HATEOAS links).
/// </summary>
public class WordIndexDto
{
    public string Tconst { get; set; } = default!;
    public string Word { get; set; } = default!;
    public string Field { get; set; } = default!;
    public string? Lexeme { get; set; }
    public List<LinkDto> Links { get; set; } = new();
}