using CapoBooking.Data;
using CapoBooking.Domain;
using CapoBooking.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapoBooking.Controllers;

[ApiController]
[Route("api/services")]
public class ServicesController : ControllerBase
{
    private readonly AppDbContext _db;
    
    public ServicesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllServices()
    {
        var services = await _db.Services.ToListAsync();

        if (services == null)
        {
            return NotFound();
        }

        return Ok(services);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Service>> GetService(int id)
    {
        var service = await _db.Services.FindAsync(id);

        if (service == null)
        {
            return NotFound();
        }

        return Ok(service);
    }

    [HttpPost]
    public async Task<IActionResult> CreateService(CreateServiceRequest request)
    {
        var service = new Service()
        {
            Name = request.Name,
            DurationMinutes = request.DurationMinutes,
            Price = request.Price
        };

        _db.Services.Add(service);
        await _db.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Service>> DeleteService(int id)
    {
        
        var service = await _db.Services.FindAsync(id);

        if (service == null)
            return NotFound();

        _db.Services.Remove(service);
        await _db.SaveChangesAsync();

        return Ok();
    }
    
}