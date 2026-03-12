using System.ComponentModel.DataAnnotations;

namespace CapoBooking.DTOs;

public class CreateServiceRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required]
    public int DurationMinutes { get; set; }
    [Required]
    public decimal Price { get; set; }
    
}