using System.ComponentModel.DataAnnotations;

namespace CIT_Portfolio_Project_API.Models.Entities;

public class UserSearchHistory
{
    public int Id { get; set; }
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }

    [Required]
    [StringLength(512)]
    public string Text { get; set; } = default!;
    public DateTime SearchedAt { get; set; }
}
