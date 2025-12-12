using System.ComponentModel.DataAnnotations;

namespace CIT_Portfolio_Project_API.Models.Entities;

public class MovieGenre
{
    [Required]
    [StringLength(20)]
    [RegularExpression("^tt\\d{3,}$", ErrorMessage = "Tconst must look like 'tt123' (IMDB key)")]
    public string Tconst { get; set; } = default!;

    [Required]
    [StringLength(100)]
    public string Genre { get; set; } = default!;
}
