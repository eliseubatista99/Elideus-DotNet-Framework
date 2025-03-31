using Microsoft.IdentityModel.Tokens;

namespace ElideusDotNetFramework.Core
{
    public class TokenConfiguration
    {
        public required SymmetricSecurityKey Key { get; set; }

        public required string Issuer { get; set; }

        public required string Audience { get; set; }

        public required int LifeTime { get; set; }
    }
}
