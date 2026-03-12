using CapoBooking.Data;
using CapoBooking.Domain;
using CapoBooking.DTOs;
using Microsoft.AspNetCore.Mvc;

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
    
}