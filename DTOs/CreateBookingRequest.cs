using System.ComponentModel.DataAnnotations;

namespace CapoBooking.DTOs;

public class CreateBookingRequest
{
    [Range(1, int.MaxValue)]
    public int ServiceId { get; set; }

    [Required]
    [MaxLength(100)]
    public string ClientName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string ClientEmail { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string ClientMobile { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
}
