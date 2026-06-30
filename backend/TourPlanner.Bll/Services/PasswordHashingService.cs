using System.Security.Cryptography;
using TourPlanner.Bll.Interfaces;

namespace TourPlanner.Bll.Services
{
    public class PasswordHashingService : IPasswordHashingService
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;
        private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

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
                return false;
            }
        }
    }
}
