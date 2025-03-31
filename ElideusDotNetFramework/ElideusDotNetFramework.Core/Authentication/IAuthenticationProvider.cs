using Microsoft.IdentityModel.Tokens;

namespace ElideusDotNetFramework.Core
{
    public interface IAuthenticationProvider
    {
        public TokenValidationParameters GetTokenValidationParameters();

        public (TokenData token, TokenData refreshToken) GenerateTokens(string id);

        public (bool isValid, DateTime expirationTime) IsValidToken(string token);
    }
}
