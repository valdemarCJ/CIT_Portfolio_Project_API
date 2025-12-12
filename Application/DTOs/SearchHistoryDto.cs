namespace CIT_Portfolio_Project_API.Application.DTOs;

/// <summary>
/// A user's past search (text + timestamp) with an optional 'repeat' link.
/// </summary>
public class SearchHistoryDto
{
    public string Text { get; set; } = default!;
    public DateTime SearchedAt { get; set; }
    public List<LinkDto> Links { get; set; } = new();
}
