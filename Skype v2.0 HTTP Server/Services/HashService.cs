namespace HttpServer.Services
{
    using System;
    using System.Security.Cryptography;

    using HttpServer.Services.Interfaces;

    using Microsoft.AspNetCore.Cryptography.KeyDerivation;

    public class HashService : IHashService
    {
        private const int HashIterations = 10_000;

        public string HashPassword(string rawPassword, byte[] salt)
        {
            byte[] hashedPassword = KeyDerivation.Pbkdf2(rawPassword, salt, KeyDerivationPrf.HMACSHA256, HashIterations, 256 / 8);

            return Convert.ToBase64String(hashedPassword);
        }

        public byte[] GenerateSalt(int bits = 128)
        {
            byte[] salt = new byte[bits / 8];

            using (RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(salt);
            }

            return salt;
        }
    }
}