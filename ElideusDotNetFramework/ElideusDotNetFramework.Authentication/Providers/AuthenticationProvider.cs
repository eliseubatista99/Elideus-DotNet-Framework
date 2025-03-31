using ElideusDotNetFramework.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace ElideusDotNetFramework.Authentication
{
    [ExcludeFromCodeCoverage]
    public class AuthenticationProvider : IAuthenticationProvider
    {
        protected IApplicationContext applicationContext;

        public AuthenticationProvider(IApplicationContext _applicationContext)
        {
            applicationContext = _applicationContext;
        }
  
        protected virtual (SymmetricSecurityKey key, string issuer, string audience, DateTime expireDateTime, int lifeTime) GetTokenConfiguration()
        {
            return (new SymmetricSecurityKey(Encoding.UTF8.GetBytes(string.Empty)), string.Empty, string.Empty, DateTime.UtcNow, 0);
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

        public void AddAuthenticationToApplicationBuilder(ref WebApplicationBuilder builder)
        {
            var authConfigs = GetTokenConfiguration();

            // Add the process of verifying who they are
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = authConfigs.issuer,
                    ValidAudience = authConfigs.audience,
                    IssuerSigningKey = authConfigs.key,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
            });
        }

        public void AddAuthorizationToSwaggerGen(ref WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }

        public (string token, DateTime expirationTime) GenerateToken(string id)
        {
            var tokenConfig = GetTokenConfiguration();
            var generationConfigs = GetGenerationConfigs(id);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(generationConfigs.claims),
                Expires = tokenConfig.expireDateTime,
                Issuer = tokenConfig.issuer,
                Audience = tokenConfig.audience,
                SigningCredentials = new SigningCredentials(tokenConfig.key, generationConfigs.securityAlgorithm),

            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(tokenDescriptor);

            return (handler.WriteToken(securityToken), tokenConfig.expireDateTime);
        }

        public (bool isValid, DateTime expirationTime) IsValidToken(string token)
        {
            try
            {
                var tokenConfig = GetTokenConfiguration();

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = tokenConfig.key,
                    ValidateLifetime = false,
                    ValidIssuer = tokenConfig.issuer,
                    ValidAudience = tokenConfig.audience,
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

                var jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return (false, DateTime.Now);
                }

                return (true, jwtSecurityToken.ValidTo);
            }
            catch (Exception ex)
            {
                return (false, DateTime.Now);
            }
        }

        public int GetTokenLifeTime()
        {
            var tokenConfig = GetTokenConfiguration();

            return tokenConfig.lifeTime;
        }
    }
}
