using System;
using System.Security.Cryptography;
using System.Text;

namespace StudentManagement
{
    public static class PasswordHelper
    {
        private const int SaltSize = 32;
        private const int HashSize = 32;
        private const int Iterations = 100000;

        public static (string hash, string salt) HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hash;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                hash = pbkdf2.GetBytes(HashSize);
            }

            return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
        }

        public static bool VerifyPassword(string password, string saltBase64, string hashBase64)
        {
            byte[] saltBytes = Convert.FromBase64String(saltBase64);
            byte[] hashBytes;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256))
            {
                hashBytes = pbkdf2.GetBytes(HashSize);
            }
            return hashBase64 == Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyLegacyPassword(string password, string saltBase64, string hashBase64)
        {
            string combined = password + saltBase64;
            byte[] hashBytes;
            using (var sha256 = SHA256.Create())
            {
                hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
            }
            return hashBase64 == Convert.ToBase64String(hashBytes);
        }
    }
}
