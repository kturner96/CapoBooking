namespace CapoBooking.Domain;

public class Booking
{
    public int BookingId { get; set; }
    public int ServiceId { get; set; }
    public string ClientName { get; set; } = "";
    public string ClientEmail { get; set; } = "";
    public string ClientMobile { get; set; } = "";
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
}

public enum BookingStatus
{
    Pending,
    Confirmed,
    Completed,
    Cancelled,
}
