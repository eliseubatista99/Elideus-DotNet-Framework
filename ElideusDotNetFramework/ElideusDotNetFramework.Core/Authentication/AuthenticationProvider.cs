using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ElideusDotNetFramework.Core
{
    [ExcludeFromCodeCoverage]
    public class AuthenticationProvider : IAuthenticationProvider
    {
        protected IApplicationContext applicationContext;

        public AuthenticationProvider(IApplicationContext _applicationContext)
        {
            applicationContext = _applicationContext;
        }
  
        public TokenValidationParameters GetTokenValidationParameters()
        {
            var authConfigs = GetTokenConfiguration();

            return new TokenValidationParameters
            {
                ValidIssuer = authConfigs.Issuer,
                ValidAudience = authConfigs.Audience,
                IssuerSigningKey = authConfigs.Key,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };
        }

        public (TokenData token, TokenData refreshToken) GenerateTokens(string id)
        {
            var tokenConfig = GetTokenConfiguration();
            var refreshTokenConfig = GetRefreshTokenConfiguration();

            var token = GenerateToken(tokenConfig, id);
            var refreshToken = GenerateToken(refreshTokenConfig, id);

            return (token, refreshToken);
        }

        public (bool isValid, DateTime expirationTime, List<Claim> claims) IsValidToken(string token)
        {
            try
            {
                var tokenConfig = GetTokenConfiguration();

                token = CleanupToken(token);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = tokenConfig.Key,
                    ValidateLifetime = false,
                    ValidIssuer = tokenConfig.Issuer,
                    ValidAudience = tokenConfig.Audience,
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

                var jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return (false, DateTime.Now, new List<Claim>());
                }

                return (true, jwtSecurityToken.ValidTo, jwtSecurityToken.Claims.ToList());
            }
            catch (Exception ex)
            {
                return (false, DateTime.Now, new List<Claim>());
            }
        }
   
        protected virtual string CleanupToken(string token)
        {
            var tokenFirstSix = token.Substring(0, 6);

            if (tokenFirstSix == "Bearer")
            {
                token = token.Substring(7);
            }

            return token;
        }

        protected virtual TokenConfiguration GetTokenConfiguration()
        {
            return new TokenConfiguration
            {
                Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(string.Empty)),
                Audience = string.Empty,
                Issuer = string.Empty,
                LifeTime = 0,
            };
        }

        protected virtual TokenConfiguration GetRefreshTokenConfiguration()
        {
            return new TokenConfiguration
            {
                Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(string.Empty)),
                Audience = string.Empty,
                Issuer = string.Empty,
                LifeTime = 0,
            };
        }

        protected virtual (List<Claim> claims, string securityAlgorithm) GetGenerationConfigs(string id)
        {
            var claims = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(JwtRegisteredClaimNames.Name, id),
                };

            return (claims, SecurityAlgorithms.HmacSha256);
        }

        protected virtual TokenData GenerateToken(TokenConfiguration config, string id)
        {
            var generationConfigs = GetGenerationConfigs(id);

            var expireDateTime = DateTime.UtcNow.AddMinutes(config.LifeTime);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(generationConfigs.claims),
                Expires = expireDateTime,
                Issuer = config.Issuer,
                Audience = config.Audience,
                SigningCredentials = new SigningCredentials(config.Key, generationConfigs.securityAlgorithm),

            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(tokenDescriptor);

            return new TokenData
            {
                Token = handler.WriteToken(securityToken),
                ExpirationDateTime = expireDateTime,
            };
        }

    }
}
