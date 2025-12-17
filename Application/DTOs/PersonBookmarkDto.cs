namespace CIT_Portfolio_Project_API.Application.DTOs;

public class PersonBookmarkDto
{
    public string Nconst { get; set; } = default!;
    public string? Name { get; set; }
    public string? Note { get; set; }
    public List<LinkDto> Links { get; set; } = new();
}