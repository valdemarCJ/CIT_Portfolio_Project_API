using System.ComponentModel.DataAnnotations;

namespace CIT_Portfolio_Project_API.Models.Entities;

public class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(64, MinimumLength = 3)]
    public string Username { get; set; } = default!;

    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = default!;

    [Required]
    [StringLength(200, MinimumLength = 20)]
    public string PasswordHash { get; set; } = default!;
}
