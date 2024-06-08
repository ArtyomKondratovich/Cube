using System.Text.Json;
using System.Text.RegularExpressions;

namespace Cube.Services.Services
{
    public static class ValidationExtensions
    {
        public static bool IsValidEmail(this string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            var regex = new Regex(@"^\w+@\w+\.\w+$");

            return regex.IsMatch(email);
        }

        public static int PasswordCheck(this string? password)
        {
            // 0 - for null passwrod
            // 1 - easy password
            // 2 - medium password
            // 3 - strong password

            if (password == null)
            {
                return 0;
            }

            return 1;
        }

        public static bool TryParseJson(this string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return false;
            }

            try
            {
                JsonDocument.Parse(json);
                return true;
            }
            catch (System.Text.Json.JsonException)
            {
                return false;
            }
        }
    }
}
