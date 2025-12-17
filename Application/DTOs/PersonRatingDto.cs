namespace CIT_Portfolio_Project_API.Application.DTOs;

public class PersonRatingDto
{
    public string Nconst { get; set; } = default!;
    public int Value { get; set; }
    public List<LinkDto> Links { get; set; } = new();
}