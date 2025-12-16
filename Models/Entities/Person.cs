using System.ComponentModel.DataAnnotations;

namespace CIT_Portfolio_Project_API.Models.Entities;

public class Person
{
    // IMDB key (nconst)
    [Required]
    [StringLength(20)]
    [RegularExpression("^nm\\d{3,}$", ErrorMessage = "Nconst must look like 'nm123' (IMDB key)")]
    public string Nconst { get; set; } = default!;

    [StringLength(256)]
    public string? Name { get; set; }
    
    [StringLength(4)]
    public string? BirthYear { get; set; }
    
    [StringLength(4)]
    public string? DeathYear { get; set; }
    
    public decimal? NameRating { get; set; }
}
