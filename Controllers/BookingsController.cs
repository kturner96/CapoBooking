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
        try
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

        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error deleting Booking.");
        }
    }
}