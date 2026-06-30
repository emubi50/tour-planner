using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using TourPlanner.Bll.Interfaces;

namespace TourPlanner.Bll.Services
{
    public class PasswordHashingService : IPasswordHashingService
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;
        private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

        private readonly ILogger<PasswordHashingService> _logger;

        public PasswordHashingService(ILogger<PasswordHashingService> logger)
        {
            _logger = logger;
        }

        public string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);
            return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
        }

        public bool Verify(string hashedPassword, string providedPassword)
        {
            try
            {
                string[] parts = hashedPassword.Split('-');
                if (parts.Length != 2)
                {
                    _logger.LogWarning("Stored password hash is not in the expected format");
                    return false;
                }

                var salt = Convert.FromHexString(parts[1]);
                var hash = Convert.FromHexString(parts[0]);

                var providedHash = Rfc2898DeriveBytes.Pbkdf2(
                    providedPassword,
                    salt,
                    Iterations,
                    Algorithm,
                    HashSize
                );

                return CryptographicOperations.FixedTimeEquals(hash, providedHash);
            }
            catch (FormatException ex) // ex for logging purposes in the future
            {
                _logger.LogWarning(ex, "Stored password hash could not be parsed as valid hex");
                return false;
            }
        }
    }
}
