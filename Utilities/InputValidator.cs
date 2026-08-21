using System;
using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace SkillHub.Utilities
{
    public static class InputValidator
    {
        private static readonly Regex PhoneExpression =
            new Regex(@"^\+?[0-9][0-9\-\s]{6,18}$", RegexOptions.Compiled);

        public static string RequireName(string value)
        {
            string normalized = NormalizeRequired(value, "Full name", 120);

            if (normalized.Length < 2)
            {
                throw new ArgumentException("Full name must contain at least two characters.");
            }

            return normalized;
        }

        public static string RequireEmail(string value)
        {
            string normalized = NormalizeRequired(value, "Email address", 150)
                .ToLower(CultureInfo.InvariantCulture);

            try
            {
                MailAddress parsed = new MailAddress(normalized);

                if (!string.Equals(parsed.Address, normalized, StringComparison.Ordinal))
                {
                    throw new FormatException();
                }

                string domain = normalized.Substring(normalized.LastIndexOf('@') + 1);

                if (!domain.Contains(".") || domain.StartsWith(".") || domain.EndsWith("."))
                {
                    throw new FormatException();
                }

                return normalized;
            }
            catch (FormatException)
            {
                throw new ArgumentException("Enter a valid email address, such as name@example.com.");
            }
        }

        public static string NormalizeOptionalPhone(string value)
        {
            string normalized = NormalizeOptional(value, 20, "Phone number");

            if (normalized != null && !PhoneExpression.IsMatch(normalized))
            {
                throw new ArgumentException("Phone number must contain 7-20 valid digits, spaces or hyphens.");
            }

            return normalized;
        }

        public static string NormalizeOptionalAddress(string value)
        {
            return NormalizeOptional(value, 250, "Address");
        }

        public static void ValidateNewPassword(string password, string confirmation)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password is required.");
            }

            if (password.Length < 8 || password.Length > 128)
            {
                throw new ArgumentException("Password must contain between 8 and 128 characters.");
            }

            bool hasUppercase = false;
            bool hasLowercase = false;
            bool hasDigit = false;
            bool hasSymbol = false;

            foreach (char character in password)
            {
                hasUppercase |= char.IsUpper(character);
                hasLowercase |= char.IsLower(character);
                hasDigit |= char.IsDigit(character);
                hasSymbol |= !char.IsLetterOrDigit(character);
            }

            if (!hasUppercase || !hasLowercase || !hasDigit || !hasSymbol)
            {
                throw new ArgumentException(
                    "Password needs an uppercase letter, lowercase letter, number and symbol.");
            }

            if (!string.Equals(password, confirmation, StringComparison.Ordinal))
            {
                throw new ArgumentException("Password and confirmation do not match.");
            }
        }

        public static string NormalizeRequired(string value, string fieldName, int maximumLength)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(fieldName + " is required.");
            }

            string normalized = value.Trim();

            if (normalized.Length > maximumLength)
            {
                throw new ArgumentException(
                    fieldName + " cannot exceed " + maximumLength + " characters.");
            }

            return normalized;
        }

        private static string NormalizeOptional(string value, int maximumLength, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            string normalized = value.Trim();

            if (normalized.Length > maximumLength)
            {
                throw new ArgumentException(
                    fieldName + " cannot exceed " + maximumLength + " characters.");
            }

            return normalized;
        }
    }
}
