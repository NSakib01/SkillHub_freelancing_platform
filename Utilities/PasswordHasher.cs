using System;
using System.Globalization;
using System.Security.Cryptography;

namespace SkillHub.Utilities
{
    /// <summary>
    /// Password format: PBKDF2-SHA256$iterations$base64-salt$base64-key.
    /// Passwords are never stored in plain text or compared in SQL.
    /// </summary>
    public static class PasswordHasher
    {
        private const string AlgorithmName = "PBKDF2-SHA256";
        private const int IterationCount = 120000;
        private const int SaltSize = 16;
        private const int DerivedKeySize = 32;

        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password is required.", nameof(password));
            }

            byte[] salt = new byte[SaltSize];

            using (RandomNumberGenerator generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(salt);
            }

            byte[] derivedKey = DeriveKey(password, salt, IterationCount);

            return string.Join(
                "$",
                AlgorithmName,
                IterationCount.ToString(CultureInfo.InvariantCulture),
                Convert.ToBase64String(salt),
                Convert.ToBase64String(derivedKey));
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(storedHash))
            {
                return false;
            }

            string[] parts = storedHash.Split('$');
            int iterations;

            if (parts.Length != 4
                || !string.Equals(parts[0], AlgorithmName, StringComparison.Ordinal)
                || !int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out iterations)
                || iterations < 10000
                || iterations > 1000000)
            {
                return false;
            }

            try
            {
                byte[] salt = Convert.FromBase64String(parts[2]);
                byte[] expected = Convert.FromBase64String(parts[3]);

                if (salt.Length < SaltSize || expected.Length != DerivedKeySize)
                {
                    return false;
                }

                byte[] actual = DeriveKey(password, salt, iterations);
                return FixedTimeEquals(actual, expected);
            }
            catch (FormatException)
            {
                return false;
            }
            catch (CryptographicException)
            {
                return false;
            }
        }

        private static byte[] DeriveKey(string password, byte[] salt, int iterations)
        {
            using (Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256))
            {
                return deriveBytes.GetBytes(DerivedKeySize);
            }
        }

        private static bool FixedTimeEquals(byte[] first, byte[] second)
        {
            if (first == null || second == null || first.Length != second.Length)
            {
                return false;
            }

            int difference = 0;

            for (int index = 0; index < first.Length; index++)
            {
                difference |= first[index] ^ second[index];
            }

            return difference == 0;
        }
    }
}
