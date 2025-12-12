namespace CIT_Portfolio_Project_API.Application.DTOs;

/// <summary>
/// IMDB rating data representation for API responses (with HATEOAS links).
/// </summary>
public class ImdbRatingDto
{
    public string Tconst { get; set; } = default!;
    public double AverageRating { get; set; }
    public int NumVotes { get; set; }
    public List<LinkDto> Links { get; set; } = new();
}