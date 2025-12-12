using System.ComponentModel.DataAnnotations;

namespace CIT_Portfolio_Project_API.Models.Entities;

public class UserBookmark
{
    public int Id { get; set; }

    [Range(1, int.MaxValue)]
    public int UserId { get; set; }

    [Required]
    [StringLength(20)]
    [RegularExpression("^tt\\d{3,}$", ErrorMessage = "Tconst must look like 'tt123' (IMDB key)")]
    public string Tconst { get; set; } = default!;

    [StringLength(512)]
    public string? Note { get; set; }
}
