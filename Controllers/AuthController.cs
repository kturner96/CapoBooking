using System.Security.Cryptography;
using System.Text;
using CapoBooking.Data;
using CapoBooking.Domain;
using CapoBooking.DTOs;
using CapoBooking.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapoBooking.Controllers;

[ApiController]
[Route("api/admin")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;

    public AuthController(AppDbContext db)
    {
        _db = db;
    }

     [HttpPost("/login")]
     public async Task<ActionResult> Login(LoginRequestDto request)
     {
         var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

         if (user == null)
         {
             return BadRequest("User not found.");
         }

         var passwordHash = Rfc2898DeriveBytes.Pbkdf2(request.Password,
             Encoding.UTF8.GetBytes(user.PasswordSalt),
             350000,
             HashAlgorithmName.SHA512,
             64);
         
          var compareResult = CryptographicOperations.FixedTimeEquals(passwordHash, Convert.FromBase64String(user.PasswordHash));

          if (compareResult)
              return Ok("Login Successful.");

          return BadRequest("Incorrect Password.");
     }

     [HttpPost("/register")]
     public async Task<ActionResult<LoginRequest>> Register(LoginRequestDto request)
     {
         
         var hasher = new PasswordHasher();
         string salt = hasher.GenerateSalt();
         string passwordHash = hasher.GenerateHash(request.Password, salt);

         var user = new User()
         {
             Email = request.Email,
             FullName = request.FullName,
             PasswordHash = passwordHash,
             PasswordSalt = salt,
             CreatedAt = DateTime.UtcNow,
             IsActive = true,
         };

         _db.Users.Add(user);
         await _db.SaveChangesAsync();

         return CreatedAtAction(nameof(Login), new { Id = user.UserId }, user);
     }
}