using CapoBooking.Domain;

namespace CapoBooking.DTOs;

public class UpdateBookingStatusRequest
{
    public BookingStatus Status { get; set; }
}
