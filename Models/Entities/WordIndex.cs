using System.ComponentModel.DataAnnotations;

namespace CIT_Portfolio_Project_API.Models.Entities;

public class WordIndex
{
    [Required]
    [StringLength(20)]
    public string Tconst { get; set; } = default!;

    [Required]
    public string Word { get; set; } = default!;

    [Required]
    [StringLength(1)]
    public string Field { get; set; } = default!; // t=title, p=plot, c=characters, n=name

    public string? Lexeme { get; set; }
}
