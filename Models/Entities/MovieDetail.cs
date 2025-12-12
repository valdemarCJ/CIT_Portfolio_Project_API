using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CIT_Portfolio_Project_API.Models.Entities;

public class MovieDetail
{
    [Required]
    [StringLength(20)]
    [RegularExpression("^tt\\d{3,}$", ErrorMessage = "Tconst must look like 'tt123' (IMDB key)")]
    public string Tconst { get; set; } = default!;
    
    [StringLength(255)]
    public string? Awards { get; set; }
    
    public string? Plot { get; set; }
    
    [StringLength(80)]
    public string? Rated { get; set; }
    
    [StringLength(180)]
    public string? Poster { get; set; }
    
    [StringLength(100)]
    public string? Boxoffice { get; set; }
    
    public string[]? AkaTitles { get; set; }
    
    [StringLength(20)]
    public string? ParentTconst { get; set; }
    
    public int? SeasonNumber { get; set; }
    
    public int? EpisodeNumber { get; set; }
}
