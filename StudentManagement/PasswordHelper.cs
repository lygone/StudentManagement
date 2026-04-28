using System;
using System.Security.Cryptography;
using System.Text;

namespace StudentManagement
{
    public static class PasswordHelper
    {
        public static (string hash, string salt) HashPassword(string password)
        {
            byte[] saltBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);
            string hash = ComputeHash(password, salt);
            return (hash, salt);
        }

        public static bool VerifyPassword(string password, string salt, string hash)
        {
            string computedHash = ComputeHash(password, salt);
            return hash == computedHash;
        }

        private static string ComputeHash(string password, string salt)
        {
            byte[] hashBytes;
            
            string combined = password + salt;
            using (var sha256 = SHA256.Create())
            {
                hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
            }
            return Convert.ToBase64String(hashBytes);
        }
    }
}