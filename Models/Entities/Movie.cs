using System.ComponentModel.DataAnnotations;

namespace CIT_Portfolio_Project_API.Models.Entities;

public class Movie
{
    // IMDB key (tconst)
    [Required]
    [StringLength(20)]
    [RegularExpression("^tt\\d{3,}$", ErrorMessage = "Tconst must look like 'tt12345' (IMDB key)")]
    public string Tconst { get; set; } = default!;

    [StringLength(512)]
    public string? Title { get; set; }
}
