using Newtonsoft.Json;
using System.Text.Json;

namespace Cube.Application.Services
{
    public static class ValidationExtensions
    {
        public static bool IsValidEmail(this string? email)
        {
            if (email == null)
            {
                return false;
            }

            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith('.'))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
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
