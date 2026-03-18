using System.Security.Cryptography;
using System.Text;
using CapoBooking.Domain;

namespace CapoBooking.Services;

public class PasswordHasher
{
    public string GenerateHash(string password, string salt)
    {
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            Encoding.UTF8.GetBytes(salt),
            350000,
            HashAlgorithmName.SHA512,
            64
        );

        var hashedString = Convert.ToBase64String(hash);

        return hashedString;
    }

    public string GenerateSalt()
    {
        var rng = RandomNumberGenerator.Create();

        byte[] salt = new byte[64];

        rng.GetBytes(salt);

        string cryptSalt = Convert.ToBase64String(salt);

        return cryptSalt;
    }
}
