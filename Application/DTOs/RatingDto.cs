namespace CIT_Portfolio_Project_API.Application.DTOs;

/// <summary>
/// User's rating value for a movie.
/// </summary>
public class RatingDto
{
    public string Tconst { get; set; } = default!;
    public int Value { get; set; }
}
