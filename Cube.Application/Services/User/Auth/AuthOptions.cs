using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Cube.Services.Services.User.Auth
{
    public class AuthOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secrete { get; set; }
        public int TokenLifetime { get; set; }
        public SymmetricSecurityKey SummetricKey { get => new(Encoding.ASCII.GetBytes(Secrete)); }
    }
}
