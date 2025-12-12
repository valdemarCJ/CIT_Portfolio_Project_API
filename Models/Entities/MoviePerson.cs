using System.ComponentModel.DataAnnotations;

namespace CIT_Portfolio_Project_API.Models.Entities;

public class MoviePerson
{
    [Required]
    [StringLength(20)]
    [RegularExpression("^tt\\d{3,}$", ErrorMessage = "Tconst must look like 'tt123' (IMDB key)")]
    public string Tconst { get; set; } = default!;

    [Required]
    [StringLength(20)]
    [RegularExpression("^nm\\d{3,}$", ErrorMessage = "Nconst must look like 'nm123' (IMDB key)")]
    public string Nconst { get; set; } = default!;

    public string? Role { get; set; }
    
    public int Ordering { get; set; } = 0;

    [StringLength(100)]
    public string? Category { get; set; }
    
    public string? Job { get; set; }
    
    public string? Characters { get; set; }
}
