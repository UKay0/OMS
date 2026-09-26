using System.Security.Cryptography;

namespace OMS.Infrastructure.Security;

/// <summary>
/// Minimal PBKDF2 password hasher. Hand-rolled instead of pulling in the full
/// Microsoft.AspNetCore.Identity package (which brings a much bigger dependency
/// surface than a template needs just for hashing) — this is the "how" behind
/// the User.PasswordHash column that seeding and login both use.
/// Format: {iterations}.{base64 salt}.{base64 hash}
/// </summary>
public static class PasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100_000;

    public static string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public static bool Verify(string password, string hashedValue)
    {
        var parts = hashedValue.Split('.', 3);
        if (parts.Length != 3 || !int.TryParse(parts[0], out var iterations))
        {
            return false;
        }

        var salt = Convert.FromBase64String(parts[1]);
        var expectedHash = Convert.FromBase64String(parts[2]);
        var actualHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expectedHash.Length);

        return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }
}
