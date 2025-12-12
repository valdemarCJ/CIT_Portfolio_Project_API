using System.ComponentModel.DataAnnotations;

namespace CIT_Portfolio_Project_API.Models.Entities;

public class Rating
{
    [Required]
    [StringLength(20)]
    [RegularExpression("^tt\\d{3,}$", ErrorMessage = "Tconst must look like 'tt123' (IMDB key)")]
    public string Tconst { get; set; } = default!;

    [Range(0.0, 10.0)]
    public double AverageRating { get; set; }

    [Range(0, int.MaxValue)]
    public int NumVotes { get; set; }
}
