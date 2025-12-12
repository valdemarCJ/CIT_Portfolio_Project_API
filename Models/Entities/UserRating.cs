using System.ComponentModel.DataAnnotations;

namespace CIT_Portfolio_Project_API.Models.Entities;

public class UserRating
{
    public int Id { get; set; }
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }

    [Required]
    [StringLength(20)]
    [RegularExpression("^tt\\d{3,}$", ErrorMessage = "Tconst must look like 'tt123' (IMDB key)")]
    public string Tconst { get; set; } = default!;

    [Range(1, 10)]
    public int Value { get; set; }
}
