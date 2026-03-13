using CapoBooking.Data;
using CapoBooking.Domain;
using CapoBooking.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapoBooking.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly AppDbContext _db;

    public BookingsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBookings()
    {
        var bookings = await _db.Bookings.ToListAsync();
        return Ok(bookings);
        
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Booking>> GetBooking(int id)
    {
        
        var booking = await _db.Bookings.FindAsync(id);

        if (booking == null)
        {
            return NotFound();
        }
           
        return Ok(booking);

    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking(CreateBookingRequest request)
    {
        var service = await _db.Services.FindAsync(request.ServiceId);

        if (service == null)
        {
            return NotFound("Service not found.");
        }
        
        if (request.StartTime == default)
        {
            return BadRequest("Start time is required.");
        }

        if (request.StartTime < DateTime.UtcNow)
        {
            return BadRequest("Booking must be in the future.");
        }

        var newStart = request.StartTime;
        var newEnd = request.StartTime.AddMinutes(service.DurationMinutes);

        var overlappingBookingExists = await _db.Bookings.AnyAsync(b =>
            b.Status != BookingStatus.Cancelled &&
            newStart < b.EndTime &&
            newEnd < b.StartTime);

        if (overlappingBookingExists)
            return BadRequest("Booking already exists during this time.");
        

        var booking = new Booking()
        {
            ServiceId = request.ServiceId,
            ClientName = request.ClientName,
            ClientEmail = request.ClientEmail,
            ClientMobile = request.ClientMobile,
            StartTime = request.StartTime,
            EndTime = request.StartTime.AddMinutes(service.DurationMinutes)
        };

        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBooking), new { id = booking.BookingId }, booking);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Booking>> DeleteBooking(int id)
    {
            var bookingToDelete = await _db.Bookings.FindAsync(id);

            if (bookingToDelete == null)
            {
                return NotFound($"Booking with id {id} not found.");
            }

            _db.Bookings.Remove(bookingToDelete);
            await _db.SaveChangesAsync();

            return NoContent();
    }
    
    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<Booking>> UpdateBookingStatus(int id, UpdateBookingStatusRequest request)
    {
        var booking = await _db.Bookings.FindAsync(id);

        if (booking == null)
            return NotFound();

        booking.Status = request.Status;

        await _db.SaveChangesAsync();
        return Ok(booking);

    }
}