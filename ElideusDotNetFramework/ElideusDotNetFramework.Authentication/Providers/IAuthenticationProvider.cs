using Microsoft.AspNetCore.Builder;
using System.Security.Claims;

namespace ElideusDotNetFramework.Authentication
{
    public interface IAuthenticationProvider
    {
        public void AddAuthenticationToApplicationBuilder(ref WebApplicationBuilder builder);

        public void AddAuthorizationToSwaggerGen(ref WebApplicationBuilder builder);

        public (string token, DateTime expirationTime) GenerateToken(string id);

        public (bool isValid, DateTime expirationTime) IsValidToken(string token);

        public int GetTokenLifeTime();
    }
}
