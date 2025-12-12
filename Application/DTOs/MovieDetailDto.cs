namespace CIT_Portfolio_Project_API.Application.DTOs;

/// <summary>
/// Movie details representation for API responses (with HATEOAS links).
/// </summary>
public class MovieDetailDto
{
    public string Tconst { get; set; } = default!;
    public string? Awards { get; set; }
    public string? Plot { get; set; }
    public string? Rated { get; set; }
    public string? Poster { get; set; }
    public string? Boxoffice { get; set; }
    public string[]? AkaTitles { get; set; }
    public string? ParentTconst { get; set; }
    public int? SeasonNumber { get; set; }
    public int? EpisodeNumber { get; set; }
    public List<LinkDto> Links { get; set; } = new();
}