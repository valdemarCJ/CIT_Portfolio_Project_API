namespace CIT_Portfolio_Project_API.Application.DTOs;

/// <summary>
/// Complete movie representation for API responses (with HATEOAS links).
/// </summary>
public class MovieDto
{
    public string Tconst { get; set; } = default!;
    public string? TitleType { get; set; }
    public string? PrimaryTitle { get; set; }
    public string? OriginalTitle { get; set; }
    public bool? IsAdult { get; set; }
    public string? StartYear { get; set; }
    public string? EndYear { get; set; }
    public int? RuntimeMinutes { get; set; }
    public List<LinkDto> Links { get; set; } = new();

    // Legacy property for backwards compatibility
    public string? Title => PrimaryTitle;
}
