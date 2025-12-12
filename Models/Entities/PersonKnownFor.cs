using System.ComponentModel.DataAnnotations;

namespace CIT_Portfolio_Project_API.Models.Entities;

public class PersonKnownFor
{
    [Required]
    [StringLength(20)]
    [RegularExpression("^nm\\d{3,}$", ErrorMessage = "Nconst must look like 'nm123' (IMDB key)")]
    public string Nconst { get; set; } = default!;

    [Required]
    [StringLength(20)]
    [RegularExpression("^tt\\d{3,}$", ErrorMessage = "Tconst must look like 'tt123' (IMDB key)")]
    public string Tconst { get; set; } = default!;
}
