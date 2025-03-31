using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ElideusDotNetFramework.Core
{
    public interface IAuthenticationProvider
    {
        public TokenValidationParameters GetTokenValidationParameters();

        public (TokenData token, TokenData refreshToken) GenerateTokens(string id);

        public (bool isValid, DateTime expirationTime, List<Claim> claims) IsValidToken(string token);
    }
}
