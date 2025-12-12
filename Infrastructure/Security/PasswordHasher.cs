using System.Security.Cryptography;
using System.Text;

namespace CIT_Portfolio_Project_API.Infrastructure.Security;

public class PasswordHasher
{
    // Simple PBKDF2 hasher. For production, consider using ASP.NET Identity or a vetted library.
    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        var key = pbkdf2.GetBytes(32);
        return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(key);
    }

    public bool Verify(string password, string hash)
    {
        var parts = hash.Split(':');
        if (parts.Length != 2) return false;
        var salt = Convert.FromBase64String(parts[0]);
        var storedKey = Convert.FromBase64String(parts[1]);
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        var key = pbkdf2.GetBytes(32);
        return CryptographicOperations.FixedTimeEquals(key, storedKey);
    }
}
