using System.Security.Cryptography;
using System.Text;
using CapoBooking.Data;
using CapoBooking.Domain;
using CapoBooking.DTOs;
using CapoBooking.Services;
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

     [HttpPost("login")]
     public async Task<ActionResult> Login(LoginRequestDto request)
     {
         var normalisedEmail = request.Email.Trim().ToLowerInvariant();
         
         var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == normalisedEmail);

         if (user == null)
         {
             return Unauthorized("Invalid credentials.");
         }

         if (!user.IsActive)
         {
             return Unauthorized("Invalid credentials.");
         }

         var passwordHash = Rfc2898DeriveBytes.Pbkdf2(request.Password,
             Encoding.UTF8.GetBytes(user.PasswordSalt),
             350000,
             HashAlgorithmName.SHA512,
             64);
         
          var compareResult = CryptographicOperations.FixedTimeEquals(passwordHash, Convert.FromBase64String(user.PasswordHash));
          
          if (compareResult)
              return Ok("Login Successful."); // change to return JWT token 

          return Unauthorized("Invalid credentials.");
     }

     [HttpPost("register")]
     public async Task<ActionResult> Register(RegisterRequestDto request)
     {

         var normalisedEmail = request.Email.Trim().ToLowerInvariant();
         
         var existingUser = await _db.Users.AnyAsync(u => u.Email == normalisedEmail);

         if (existingUser)
             return BadRequest("Email already in use.");
         
         var hasher = new PasswordHasher();
         string salt = hasher.GenerateSalt();
         string passwordHash = hasher.GenerateHash(request.Password, salt);

         var user = new User()
         {
             Email = normalisedEmail,
             PasswordHash = passwordHash,
             FullName = request.FullName,
             PasswordSalt = salt,
             CreatedAt = DateTime.UtcNow,
             IsActive = true,
         };

         _db.Users.Add(user);
         await _db.SaveChangesAsync();

         return Ok(new { message = "User registered successfully." });
     }
}