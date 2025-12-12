namespace CIT_Portfolio_Project_API.Application.DTOs;

/// <summary>
/// A user's rating history entry (movie + value + links).
/// </summary>
public class RatingHistoryDto
{
    public string Tconst { get; set; } = default!;
    public int Value { get; set; }
    public List<LinkDto> Links { get; set; } = new();
}
