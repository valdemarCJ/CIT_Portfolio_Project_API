using System.ComponentModel.DataAnnotations;

namespace CIT_Portfolio_Project_API.Models.Entities;

public class PersonProfession
{
    [Required]
    [StringLength(20)]
    [RegularExpression("^nm\\d{3,}$", ErrorMessage = "Nconst must look like 'nm123' (IMDB key)")]
    public string Nconst { get; set; } = default!;

    [Required]
    [StringLength(64)]
    public string Profession { get; set; } = default!;
}
