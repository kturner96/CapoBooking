using System.ComponentModel.DataAnnotations;

namespace CapoBooking.DTOs;

public class RegisterRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(64)]
    [MinLength(12)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MaxLength(64)]
    public string FullName { get; set; } = string.Empty;
    

}