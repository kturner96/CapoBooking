namespace CapoBooking.Domain;

public class Service
{
    public int ServiceId { get; set; }
    public string Name { get; set; } = "";
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}