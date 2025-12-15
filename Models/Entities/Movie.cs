using System.ComponentModel.DataAnnotations;

namespace CIT_Portfolio_Project_API.Models.Entities;

public class Movie
{
    // IMDB key (tconst)
    [Required]
    [StringLength(20)]
    [RegularExpression("^tt\\d{3,}$", ErrorMessage = "Tconst must look like 'tt12345' (IMDB key)")]
    public string Tconst { get; set; } = default!;

    [StringLength(50)]
    public string? TitleType { get; set; }

    [StringLength(500)]
    public string? PrimaryTitle { get; set; }

    [StringLength(500)]
    public string? OriginalTitle { get; set; }

    public bool? IsAdult { get; set; }

    [StringLength(4)]
    public string? StartYear { get; set; }

    [StringLength(4)]
    public string? EndYear { get; set; }

    public int? RuntimeMinutes { get; set; }

    // Legacy property for backwards compatibility
    public string? Title => PrimaryTitle;
}
