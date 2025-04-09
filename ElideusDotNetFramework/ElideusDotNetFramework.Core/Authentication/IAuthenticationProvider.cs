using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ElideusDotNetFramework.Core
{
    public interface IAuthenticationProvider
    {
        public void AddValidationParameters(ref WebApplicationBuilder builder);

        public (TokenData token, TokenData refreshToken) GenerateTokens(string id);

        public (bool isValid, DateTime expirationTime, List<Claim> claims) IsValidToken(string token);

        public (bool isValid, DateTime expirationTime, List<Claim> claims) IsValidRefreshToken(string token);

    }
}
