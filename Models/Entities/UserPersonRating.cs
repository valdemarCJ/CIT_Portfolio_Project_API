using System.ComponentModel.DataAnnotations;

namespace CIT_Portfolio_Project_API.Models.Entities;

public class UserPersonRating
{
    public int Id { get; set; }
    
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }

    [Required]
    [StringLength(20)]
    [RegularExpression("^nm\\d{3,}$", ErrorMessage = "Nconst must look like 'nm123' (IMDB person key)")]
    public string Nconst { get; set; } = default!;

    [Range(1, 10)]
    public int Value { get; set; }
}